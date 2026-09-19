namespace Bank.CoreBanking.Domain.Events;

/// <summary>
/// Domain event: Posting failed
/// Published when posting to accounts could not be completed
/// Consumed by: Notifications (send failure alert), Audit (log failure)
/// </summary>
public sealed record CorePostingFailed(
    Guid TransactionId,
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount,
    string Currency,
    string FailureReason,
    int RetryCount,
    bool CanRetry,
    DateTime FailedAtUtc) : IDomainEvent
{
    public Guid AggregateId => TransactionId;
}

/// <summary>
/// Integration event published to Outbox for async consumption
/// </summary>
public sealed record CorePostingFailedIntegrationEvent(
    Guid TransactionId,
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount,
    string Currency,
    string FailureReason,
    int RetryCount,
    bool CanRetry,
    DateTime FailedAtUtc,
    DateTime OccurredAtUtc)
{
    public string EventType => nameof(CorePostingFailedIntegrationEvent);
    public string AggregateId => TransactionId.ToString();
}
