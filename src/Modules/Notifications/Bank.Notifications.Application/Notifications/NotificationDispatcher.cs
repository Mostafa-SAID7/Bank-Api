using Bank.Contracts.Notifications;
using Bank.Notifications.Domain.Notifications;

namespace Bank.Notifications.Application.Notifications;

/// <summary>
/// Implements the module's public synchronous contract. Delivery itself will
/// be performed asynchronously by the module's outbox consumer.
/// </summary>
public sealed class NotificationDispatcher : INotificationDispatchContract
{
    private readonly INotificationStore _store;
    private readonly TimeProvider _timeProvider;

    public NotificationDispatcher(INotificationStore store, TimeProvider timeProvider)
    {
        _store = store;
        _timeProvider = timeProvider;
    }

    public async Task<DispatchNotificationResult> DispatchAsync(
        DispatchNotificationRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.UserId == Guid.Empty || string.IsNullOrWhiteSpace(request.IdempotencyKey))
        {
            return new DispatchNotificationResult(Guid.Empty, false, "A user id and idempotency key are required.");
        }

        var existing = await _store.FindByIdempotencyKeyAsync(request.IdempotencyKey, cancellationToken);
        if (existing is not null)
        {
            return new DispatchNotificationResult(existing.Id, true);
        }

        var notification = new NotificationRecord(
            Guid.NewGuid(),
            request.UserId,
            request.Type,
            request.Subject,
            request.Message,
            request.Channel,
            request.Priority,
            request.IdempotencyKey,
            _timeProvider.GetUtcNow(),
            request.ScheduledAt,
            request.Data);

        var stored = await _store.AddIfAbsentAsync(notification, cancellationToken);
        return new DispatchNotificationResult(stored.Id, true);
    }
}
