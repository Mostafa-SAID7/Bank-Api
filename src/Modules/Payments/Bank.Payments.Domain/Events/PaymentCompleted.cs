namespace Bank.Payments.Domain.Events;

/// <summary>
/// Domain event: Payment was successfully completed
/// Published when payment posting to Core Banking succeeded
/// Consumed by: Notifications (send confirmation), Audit (log transaction)
/// </summary>
public sealed record PaymentCompleted(
    Guid PaymentId,
    Guid CustomerId,
    Guid FromAccountId,
    Guid BeneficiaryId,
    decimal Amount,
    string Currency,
    string TransactionReference,
    string PostingReference,
    DateTime CompletedAtUtc) : IDomainEvent
{
    public Guid AggregateId => PaymentId;
}

/// <summary>
/// Integration event published to Outbox for async consumption
/// </summary>
public sealed record PaymentCompletedIntegrationEvent(
    Guid PaymentId,
    Guid CustomerId,
    Guid FromAccountId,
    Guid BeneficiaryId,
    decimal Amount,
    string Currency,
    string TransactionReference,
    string PostingReference,
    DateTime CompletedAtUtc,
    DateTime OccurredAtUtc)
{
    public string EventType => nameof(PaymentCompletedIntegrationEvent);
    public string AggregateId => PaymentId.ToString();
}
