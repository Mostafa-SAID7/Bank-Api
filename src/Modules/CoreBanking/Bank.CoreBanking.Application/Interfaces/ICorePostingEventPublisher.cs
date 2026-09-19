namespace Bank.CoreBanking.Application;

/// <summary>
/// Publishes core posting domain events to the Outbox for async consumption
/// Enables Notifications and Audit modules to consume events without coupling
/// </summary>
public interface ICorePostingEventPublisher
{
    /// <summary>
    /// Publish a CorePostingCompleted event
    /// </summary>
    Task PublishCorePostingCompletedAsync(
        Guid transactionId,
        Guid fromAccountId,
        Guid toAccountId,
        decimal amount,
        string currency,
        decimal fromAccountNewBalance,
        decimal toAccountNewBalance,
        DateTime postedAtUtc,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Publish a CorePostingFailed event
    /// </summary>
    Task PublishCorePostingFailedAsync(
        Guid transactionId,
        Guid fromAccountId,
        Guid toAccountId,
        decimal amount,
        string currency,
        string failureReason,
        int retryCount,
        bool canRetry,
        DateTime failedAtUtc,
        CancellationToken cancellationToken = default);
}
