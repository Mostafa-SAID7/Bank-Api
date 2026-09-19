using Bank.Contracts.Notifications;

namespace Bank.Notifications.Application.Notifications;

public sealed class NotificationManagementService(INotificationReadStore store) : INotificationManagementContract
{
    public Task<IReadOnlyList<NotificationHistoryEntry>> GetHistoryAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default) =>
        store.GetHistoryAsync(userId, Math.Max(page, 1), Math.Clamp(pageSize, 1, 100), cancellationToken);

    public Task<int> GetUnreadCountAsync(Guid userId, CancellationToken cancellationToken = default) =>
        store.GetUnreadCountAsync(userId, cancellationToken);

    public Task<bool> MarkAsReadAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken = default) =>
        store.MarkAsReadAsync(notificationId, userId, cancellationToken);

    public Task<NotificationPreferences?> GetPreferencesAsync(Guid userId, CancellationToken cancellationToken = default) =>
        store.GetPreferencesAsync(userId, cancellationToken);

    public Task SavePreferencesAsync(Guid userId, NotificationPreferences preferences, CancellationToken cancellationToken = default) =>
        store.SavePreferencesAsync(userId, preferences, cancellationToken);
}
