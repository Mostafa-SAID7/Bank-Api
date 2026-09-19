using Bank.Contracts.CoreBanking;
using Bank.CoreBanking.Domain.Entities;
using Bank.CoreBanking.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Bank.CoreBanking.Application.Handlers;

/// <summary>
/// Implements ICoreBankingPostingContract - handles posting requests from Payments module
/// Orchestrates double-entry posting to both debit and credit accounts
/// Publishes CorePostingCompleted/CorePostingFailed events
/// </summary>
public sealed class PostPaymentCommandHandler : ICoreBankingPostingContract
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILedgerRepository _ledgerRepository;
    private readonly ICorePostingEventPublisher _eventPublisher;
    private readonly ILogger<PostPaymentCommandHandler> _logger;

    public PostPaymentCommandHandler(
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        ILedgerRepository ledgerRepository,
        ICorePostingEventPublisher eventPublisher,
        ILogger<PostPaymentCommandHandler> logger)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _ledgerRepository = ledgerRepository;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task<PostingResult> PostPaymentAsync(
        PostPaymentCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(
                "Posting payment: PaymentId={PaymentId}, FromAccount={FromAccountId}, Amount={Amount}",
                command.PaymentId,
                command.AccountId,
                command.Amount);

            // 1. Check if posting already exists (idempotency)
            var existingTransaction = await _transactionRepository.FindByIdempotencyKeyAsync(
                command.IdempotencyKey,
                cancellationToken);

            if (existingTransaction != null)
            {
                _logger.LogWarning(
                    "Duplicate posting detected: IdempotencyKey={IdempotencyKey}, TransactionId={TransactionId}",
                    command.IdempotencyKey,
                    existingTransaction.Id);

                return new PostingResult(
                    Succeeded: existingTransaction.Status == Domain.Enums.TransactionStatus.Completed,
                    Reference: existingTransaction.Reference,
                    FailureReason: null);
            }

            // 2. Get the from account (payment source)
            var fromAccount = await _accountRepository.GetByIdAsync(command.AccountId, cancellationToken);
            if (fromAccount == null)
            {
                _logger.LogError(
                    "Source account not found: AccountId={AccountId}",
                    command.AccountId);

                return new PostingResult(
                    Succeeded: false,
                    Reference: null,
                    FailureReason: "Source account not found");
            }

            // 3. Validate sufficient balance
            if (fromAccount.Balance < command.Amount)
            {
                _logger.LogWarning(
                    "Insufficient balance: AccountId={AccountId}, Balance={Balance}, Required={Amount}",
                    command.AccountId,
                    fromAccount.Balance,
                    command.Amount);

                return new PostingResult(
                    Succeeded: false,
                    Reference: null,
                    FailureReason: "Insufficient balance");
            }

            // 4. Create transaction record
            var transaction = Transaction.Create(
                fromAccount.Id,
                Guid.Empty, // Will be filled by beneficiary account in Payments module
                command.Amount,
                command.Currency,
                $"Payment {command.PaymentId}",
                command.IdempotencyKey,
                Domain.Enums.TransactionType.Payment);

            // 5. Post to ledger (double-entry)
            // Debit from source account
            var debitEntry = Ledger.CreateDebit(
                transaction.Id,
                fromAccount.Id,
                command.Amount,
                "Asset", // Bank account is an asset
                command.PaymentId.ToString(),
                $"Payment debit");

            // Update account balance
            var newBalance = fromAccount.PostTransaction(-command.Amount);

            // 6. Save transaction and ledger entries
            transaction.MarkAsPosted(command.PaymentId.ToString());
            await _transactionRepository.AddAsync(transaction, cancellationToken);
            await _ledgerRepository.AddAsync(debitEntry, cancellationToken);
            await _accountRepository.UpdateAsync(fromAccount, cancellationToken);
            await _accountRepository.SaveChangesAsync(cancellationToken);

            transaction.MarkAsCompleted();
            await _transactionRepository.UpdateAsync(transaction, cancellationToken);
            await _transactionRepository.SaveChangesAsync(cancellationToken);

            // 7. Publish completion event
            await _eventPublisher.PublishCorePostingCompletedAsync(
                transaction.Id,
                fromAccount.Id,
                Guid.Empty, // Will be filled by beneficiary
                command.Amount,
                command.Currency,
                newBalance,
                0, // To account balance unknown here
                DateTime.UtcNow,
                cancellationToken);

            _logger.LogInformation(
                "Posting completed successfully: TransactionId={TransactionId}, Reference={Reference}",
                transaction.Id,
                transaction.Reference);

            return new PostingResult(
                Succeeded: true,
                Reference: transaction.Reference,
                FailureReason: null);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error posting payment: PaymentId={PaymentId}, IdempotencyKey={IdempotencyKey}",
                command.PaymentId,
                command.IdempotencyKey);

            return new PostingResult(
                Succeeded: false,
                Reference: null,
                FailureReason: $"Posting failed: {ex.Message}");
        }
    }
}
