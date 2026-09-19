namespace Bank.CoreBanking.Domain.Events;

/// <summary>
/// Domain event: Posting was successfully completed
/// Published when transaction has been posted to both accounts
/// Consumed by: Notifications (send confirmation), Audit (log posting)
/// </summary>
public sealed record CorePostingCompleted(
    Guid TransactionId,
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount,
    string Currency,
    decimal FromAccountNewBalance,
    decimal ToAccountNewBalance,
    DateTime PostedAtUtc) : IDomainEvent
{
    public Guid AggregateId => TransactionId;
}

/// <summary>
/// Integration event published to Outbox for async consumption
/// </summary>
public sealed record CorePostingCompletedIntegrationEvent(
    Guid TransactionId,
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount,
    string Currency,
    decimal FromAccountNewBalance,
    decimal ToAccountNewBalance,
    DateTime PostedAtUtc,
    DateTime OccurredAtUtc)
{
    public string EventType => nameof(CorePostingCompletedIntegrationEvent);
    public string AggregateId => TransactionId.ToString();
}
