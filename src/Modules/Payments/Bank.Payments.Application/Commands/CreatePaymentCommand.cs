using MediatR;

namespace Bank.Payments.Application.Commands;

/// <summary>
/// Command to create a new payment
/// Will request posting from Core Banking module via contract
/// </summary>
public sealed record CreatePaymentCommand(
    Guid CustomerId,
    Guid FromAccountId,
    Guid BeneficiaryId,
    decimal Amount,
    string Currency,
    string IdempotencyKey,
    string? Description = null) : IRequest<CreatePaymentResult>;

public sealed record CreatePaymentResult(
    bool Success,
    Guid? PaymentId,
    string? Message,
    string? ErrorCode = null);
