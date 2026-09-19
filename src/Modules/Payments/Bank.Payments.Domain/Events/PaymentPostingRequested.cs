namespace Bank.Payments.Domain.Events;

/// <summary>
/// Domain event: Payment posting was requested from Payments module to Core Banking
/// This event is raised after a payment command is validated
/// </summary>
public sealed record PaymentPostingRequested(
    Guid PaymentId,
    Guid AccountId,
    decimal Amount,
    string Currency,
    string IdempotencyKey,
    DateTime OccurredAtUtc)
{
    public Guid AggregateId => PaymentId;
    public int Version => 1;
}
