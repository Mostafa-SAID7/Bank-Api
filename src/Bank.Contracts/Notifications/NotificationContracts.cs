namespace Bank.Contracts.Notifications;

/// <summary>
/// The supported delivery mechanisms for a notification. This contract is
/// deliberately independent from a module's persistence model.
/// </summary>
public enum NotificationChannel
{
    InApp = 1,
    Email = 2,
    SMS = 3,
    Push = 4
}

public enum NotificationPriority
{
    Low = 1,
    Normal = 2,
    High = 3,
    Critical = 4
}

public enum NotificationDeliveryStatus
{
    Pending = 1,
    Sent = 2,
    Delivered = 3,
    Read = 4,
    Failed = 5
}

/// <summary>
/// A request from another module to deliver a notification. The idempotency
/// key makes a retry safe when callers publish the same business event again.
/// </summary>
public sealed record DispatchNotificationRequest(
    Guid UserId,
    string Type,
    string Subject,
    string Message,
    NotificationChannel Channel,
    NotificationPriority Priority,
    string IdempotencyKey,
    DateTimeOffset? ScheduledAt = null,
    IReadOnlyDictionary<string, string>? Data = null);

public sealed record DispatchNotificationResult(
    Guid NotificationId,
    bool Accepted,
    string? FailureReason = null);

/// <summary>
/// The narrowly scoped synchronous contract exposed by Notifications. Other
/// modules must not access the Notifications database or its internal types.
/// </summary>
public interface INotificationDispatchContract
{
    Task<DispatchNotificationResult> DispatchAsync(
        DispatchNotificationRequest request,
        CancellationToken cancellationToken = default);
}

public sealed record NotificationHistoryEntry(
    Guid Id,
    string Type,
    string Subject,
    string Message,
    NotificationChannel Channel,
    NotificationDeliveryStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? SentAt,
    DateTimeOffset? ReadAt,
    string? ErrorMessage);

public sealed record NotificationPreferences(
    bool TransactionAlerts,
    bool SecurityAlerts,
    bool LowBalanceAlerts,
    bool PaymentReminders,
    bool MarketingNotifications,
    decimal TransactionAlertThreshold,
    decimal LowBalanceThreshold,
    IReadOnlyList<NotificationChannel> PreferredChannels,
    string? PhoneNumber,
    string? Email,
    string Language,
    string TimeZone);

public interface INotificationManagementContract
{
    Task<IReadOnlyList<NotificationHistoryEntry>> GetHistoryAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> MarkAsReadAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken = default);
    Task<NotificationPreferences?> GetPreferencesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task SavePreferencesAsync(Guid userId, NotificationPreferences preferences, CancellationToken cancellationToken = default);
}

/// <summary>
/// Published after a notification has been accepted for delivery. Consumers
/// should use EventId to make processing idempotent.
/// </summary>
public sealed record NotificationAcceptedIntegrationEvent(
    Guid EventId,
    Guid NotificationId,
    Guid UserId,
    string Type,
    NotificationChannel Channel,
    DateTimeOffset OccurredAt) : IIntegrationEvent;
