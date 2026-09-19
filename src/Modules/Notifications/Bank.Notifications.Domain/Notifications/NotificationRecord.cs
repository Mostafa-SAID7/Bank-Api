using Bank.Contracts.Notifications;

namespace Bank.Notifications.Domain.Notifications;

/// <summary>
/// The Notifications module's persistence-agnostic aggregate. A user id is an
/// external reference owned by Identity; this module deliberately has no User
/// navigation property and therefore no dependency on Identity's model.
/// </summary>
public sealed class NotificationRecord
{
    public NotificationRecord(
        Guid id,
        Guid userId,
        string type,
        string subject,
        string message,
        NotificationChannel channel,
        NotificationPriority priority,
        string idempotencyKey,
        DateTimeOffset createdAt,
        DateTimeOffset? scheduledAt,
        IReadOnlyDictionary<string, string>? data)
    {
        Id = id;
        UserId = userId;
        Type = type;
        Subject = subject;
        Message = message;
        Channel = channel;
        Priority = priority;
        IdempotencyKey = idempotencyKey;
        CreatedAt = createdAt;
        ScheduledAt = scheduledAt;
        Data = data;
    }

    public Guid Id { get; }
    public Guid UserId { get; }
    public string Type { get; }
    public string Subject { get; }
    public string Message { get; }
    public NotificationChannel Channel { get; }
    public NotificationPriority Priority { get; }
    public string IdempotencyKey { get; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset? ScheduledAt { get; }
    public IReadOnlyDictionary<string, string>? Data { get; }
}
