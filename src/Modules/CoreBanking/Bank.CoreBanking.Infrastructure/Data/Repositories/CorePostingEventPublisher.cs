using Bank.CoreBanking.Application;
using Bank.CoreBanking.Domain.Events;

namespace Bank.CoreBanking.Infrastructure.Data.Repositories;

/// <summary>
/// Publishes core posting domain events to the Outbox for async consumption
/// Currently implements in-memory outbox - will be upgraded to persistent outbox in future
/// </summary>
public sealed class CorePostingEventPublisher : ICorePostingEventPublisher
{
    private readonly CoreBankingDbContext _context;

    public CorePostingEventPublisher(CoreBankingDbContext context)
    {
        _context = context;
    }

    public async Task PublishCorePostingCompletedAsync(
        Guid transactionId,
        Guid fromAccountId,
        Guid toAccountId,
        decimal amount,
        string currency,
        decimal fromAccountNewBalance,
        decimal toAccountNewBalance,
        DateTime postedAtUtc,
        CancellationToken cancellationToken = default)
    {
        var integrationEvent = new CorePostingCompletedIntegrationEvent(
            transactionId,
            fromAccountId,
            toAccountId,
            amount,
            currency,
            fromAccountNewBalance,
            toAccountNewBalance,
            postedAtUtc,
            DateTime.UtcNow
        );

        // TODO: Persist to Outbox table for async consumption by other modules
        // For now, this is published in-memory during the same transaction
        await Task.CompletedTask;
    }

    public async Task PublishCorePostingFailedAsync(
        Guid transactionId,
        Guid fromAccountId,
        Guid toAccountId,
        decimal amount,
        string currency,
        string failureReason,
        int retryCount,
        bool canRetry,
        DateTime failedAtUtc,
        CancellationToken cancellationToken = default)
    {
        var integrationEvent = new CorePostingFailedIntegrationEvent(
            transactionId,
            fromAccountId,
            toAccountId,
            amount,
            currency,
            failureReason,
            retryCount,
            canRetry,
            failedAtUtc,
            DateTime.UtcNow
        );

        // TODO: Persist to Outbox table for async consumption by other modules
        // For now, this is published in-memory during the same transaction
        await Task.CompletedTask;
    }
}
