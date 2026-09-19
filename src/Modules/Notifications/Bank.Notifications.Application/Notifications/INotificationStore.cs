using Bank.Notifications.Domain.Notifications;

namespace Bank.Notifications.Application.Notifications;

/// <summary>
/// Notifications-owned persistence port. Its eventual EF Core implementation
/// will use only tables in the notifications schema.
/// </summary>
public interface INotificationStore
{
    Task<NotificationRecord?> FindByIdempotencyKeyAsync(
        string idempotencyKey,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Stores a notification only if its idempotency key is new and returns
    /// the canonical record for that key.
    /// </summary>
    Task<NotificationRecord> AddIfAbsentAsync(
        NotificationRecord notification,
        CancellationToken cancellationToken = default);
}
