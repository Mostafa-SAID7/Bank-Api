using Bank.Notifications.Domain.Notifications;

namespace Bank.Notifications.Application.Notifications;

/// <summary>
/// A deterministic store for local development and module-level tests. The
/// production registration will be an EF Core adapter backed by the
/// notifications schema; callers use only <see cref="INotificationStore"/>.
/// </summary>
public sealed class InMemoryNotificationStore : INotificationStore
{
    private readonly Dictionary<string, NotificationRecord> _notifications = new(StringComparer.Ordinal);
    private readonly Lock _lock = new();

    public Task<NotificationRecord?> FindByIdempotencyKeyAsync(
        string idempotencyKey,
        CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            _notifications.TryGetValue(idempotencyKey, out var notification);
            return Task.FromResult(notification);
        }
    }

    public Task<NotificationRecord> AddIfAbsentAsync(
        NotificationRecord notification,
        CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            if (_notifications.TryGetValue(notification.IdempotencyKey, out var existing))
            {
                return Task.FromResult(existing);
            }

            _notifications.Add(notification.IdempotencyKey, notification);
        }

        return Task.FromResult(notification);
    }
}
