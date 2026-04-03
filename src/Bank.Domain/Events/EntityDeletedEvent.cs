using Bank.Domain.Common;

namespace Bank.Domain.Events;

/// <summary>
/// Domain event published when an entity is deleted
/// </summary>
public class EntityDeletedEvent : BaseDomainEvent
{
    public Guid? UserId { get; }
    public string EntityType { get; }
    public string EntityId { get; }
    public string? OldValues { get; }
    public string? IpAddress { get; }
    public string? UserAgent { get; }
    public string? SessionId { get; }
    public string? RequestId { get; }

    public EntityDeletedEvent(
        Guid? userId,
        string entityType,
        string entityId,
        string? oldValues = null,
        string? ipAddress = null,
        string? userAgent = null,
        string? sessionId = null,
        string? requestId = null)
    {
        UserId = userId;
        EntityType = entityType;
        EntityId = entityId;
        OldValues = oldValues;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        SessionId = sessionId;
        RequestId = requestId;
    }
}
