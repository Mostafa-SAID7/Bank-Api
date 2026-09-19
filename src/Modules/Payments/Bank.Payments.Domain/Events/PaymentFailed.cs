namespace Bank.Payments.Domain.Events;

/// <summary>
/// Domain event: Payment processing failed
/// Published when payment posting to Core Banking failed
/// Consumed by: Notifications (send failure alert), Audit (log failure)
/// </summary>
public sealed record PaymentFailed(
    Guid PaymentId,
    Guid CustomerId,
    Guid FromAccountId,
    Guid BeneficiaryId,
    decimal Amount,
    string Currency,
    string FailureReason,
    int RetryCount,
    bool CanRetry,
    DateTime FailedAtUtc) : IDomainEvent
{
    public Guid AggregateId => PaymentId;
}

/// <summary>
/// Integration event published to Outbox for async consumption
/// </summary>
public sealed record PaymentFailedIntegrationEvent(
    Guid PaymentId,
    Guid CustomerId,
    Guid FromAccountId,
    Guid BeneficiaryId,
    decimal Amount,
    string Currency,
    string FailureReason,
    int RetryCount,
    bool CanRetry,
    DateTime FailedAtUtc,
    DateTime OccurredAtUtc)
{
    public string EventType => nameof(PaymentFailedIntegrationEvent);
    public string AggregateId => PaymentId.ToString();
}
