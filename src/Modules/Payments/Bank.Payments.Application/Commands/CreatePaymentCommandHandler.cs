using Bank.Contracts.CoreBanking;
using Bank.Payments.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bank.Payments.Application.Commands;

/// <summary>
/// Handler for CreatePaymentCommand
/// Orchestrates: Validation → Payment creation → Core Banking posting contract → Event publishing
/// </summary>
public sealed class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, CreatePaymentResult>
{
    private readonly IPaymentsRepository _paymentsRepository;
    private readonly IBeneficiaryRepository _beneficiaryRepository;
    private readonly ICoreBankingPostingContract _postingContract;
    private readonly IPaymentEventPublisher _eventPublisher;
    private readonly ILogger<CreatePaymentCommandHandler> _logger;

    public CreatePaymentCommandHandler(
        IPaymentsRepository paymentsRepository,
        IBeneficiaryRepository beneficiaryRepository,
        ICoreBankingPostingContract postingContract,
        IPaymentEventPublisher eventPublisher,
        ILogger<CreatePaymentCommandHandler> logger)
    {
        _paymentsRepository = paymentsRepository;
        _beneficiaryRepository = beneficiaryRepository;
        _postingContract = postingContract;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task<CreatePaymentResult> Handle(
        CreatePaymentCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation(
                "Processing payment: Account={AccountId}, Amount={Amount}, IdempotencyKey={IdempotencyKey}",
                request.FromAccountId,
                request.Amount,
                request.IdempotencyKey);

            // 1. Check if payment already exists (idempotency)
            var existing = await _paymentsRepository.FindByIdempotencyKeyAsync(
                request.IdempotencyKey,
                cancellationToken);
            
            if (existing != null)
            {
                _logger.LogWarning(
                    "Duplicate payment detected: IdempotencyKey={IdempotencyKey}, PaymentId={PaymentId}",
                    request.IdempotencyKey,
                    existing.Id);
                
                return new CreatePaymentResult(
                    Success: existing.Status == PaymentStatus.Completed,
                    PaymentId: existing.Id,
                    Message: "Payment already processed",
                    ErrorCode: "DUPLICATE_PAYMENT");
            }

            // 2. Validate beneficiary exists and is active
            var beneficiary = await _beneficiaryRepository.GetByIdAsync(
                request.BeneficiaryId,
                cancellationToken);

            if (beneficiary == null || !beneficiary.IsActive)
            {
                _logger.LogWarning(
                    "Invalid beneficiary: BeneficiaryId={BeneficiaryId}",
                    request.BeneficiaryId);
                
                return new CreatePaymentResult(
                    Success: false,
                    PaymentId: null,
                    Message: "Beneficiary not found or inactive",
                    ErrorCode: "INVALID_BENEFICIARY");
            }

            // 3. Create payment aggregate
            var payment = Payment.Create(
                request.CustomerId,
                request.FromAccountId,
                request.BeneficiaryId,
                request.Amount,
                request.Currency,
                request.IdempotencyKey,
                request.Description);

            // 4. Request posting from Core Banking
            _logger.LogInformation(
                "Requesting posting from Core Banking: PaymentId={PaymentId}, Amount={Amount}",
                payment.Id,
                request.Amount);

            var postingCommand = new PostPaymentCommand(
                payment.Id,
                request.FromAccountId,
                request.Amount,
                request.Currency,
                request.IdempotencyKey);

            var postingResult = await _postingContract.PostPaymentAsync(
                postingCommand,
                cancellationToken);

            if (!postingResult.Succeeded)
            {
                _logger.LogError(
                    "Core Banking posting failed: PaymentId={PaymentId}, Reason={Reason}",
                    payment.Id,
                    postingResult.FailureReason);

                payment.MarkAsFailed(postingResult.FailureReason ?? "Unknown posting error");
                await _paymentsRepository.AddAsync(payment, cancellationToken);
                await _paymentsRepository.SaveChangesAsync(cancellationToken);

                // Publish failure event for Notifications and Audit
                await _eventPublisher.PublishPaymentFailedAsync(
                    payment.Id,
                    request.CustomerId,
                    request.FromAccountId,
                    request.BeneficiaryId,
                    request.Amount,
                    request.Currency,
                    postingResult.FailureReason ?? "Unknown posting error",
                    payment.RetryCount,
                    payment.CanRetry(),
                    cancellationToken);

                return new CreatePaymentResult(
                    Success: false,
                    PaymentId: payment.Id,
                    Message: postingResult.FailureReason ?? "Posting failed",
                    ErrorCode: "POSTING_FAILED");
            }

            // 5. Update payment with posting reference
            payment.MarkAsPosted(postingResult.Reference!);
            payment.MarkAsCompleted();

            // 6. Record transfer on beneficiary
            beneficiary.RecordTransfer(request.Amount);

            // 7. Save changes and publish events
            await _paymentsRepository.AddAsync(payment, cancellationToken);
            await _beneficiaryRepository.UpdateAsync(beneficiary, cancellationToken);
            await _paymentsRepository.SaveChangesAsync(cancellationToken);

            // Publish completion event for Notifications and Audit
            await _eventPublisher.PublishPaymentCompletedAsync(
                payment.Id,
                request.CustomerId,
                request.FromAccountId,
                request.BeneficiaryId,
                request.Amount,
                request.Currency,
                payment.TransactionReference!,
                postingResult.Reference!,
                cancellationToken);

            _logger.LogInformation(
                "Payment completed successfully: PaymentId={PaymentId}, Reference={Reference}",
                payment.Id,
                postingResult.Reference);

            return new CreatePaymentResult(
                Success: true,
                PaymentId: payment.Id,
                Message: "Payment processed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error processing payment: IdempotencyKey={IdempotencyKey}",
                request.IdempotencyKey);

            return new CreatePaymentResult(
                Success: false,
                PaymentId: null,
                Message: $"Payment processing failed: {ex.Message}",
                ErrorCode: "INTERNAL_ERROR");
        }
    }
}
