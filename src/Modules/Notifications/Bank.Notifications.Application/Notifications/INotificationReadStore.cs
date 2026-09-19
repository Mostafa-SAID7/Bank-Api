using Bank.Contracts.Notifications;

namespace Bank.Notifications.Application.Notifications;

public interface INotificationReadStore
{
    Task<IReadOnlyList<NotificationHistoryEntry>> GetHistoryAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> MarkAsReadAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken = default);
    Task<NotificationPreferences?> GetPreferencesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task SavePreferencesAsync(Guid userId, NotificationPreferences preferences, CancellationToken cancellationToken = default);
}
