using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Events;

/// <summary>
/// Domain event published when an entity is created
/// </summary>
public class EntityCreatedEvent : BaseDomainEvent
{
    public Guid? UserId { get; }
    public string EntityType { get; }
    public string EntityId { get; }
    public string NewValues { get; }
    public string? IpAddress { get; }
    public string? UserAgent { get; }
    public string? SessionId { get; }
    public string? RequestId { get; }

    public EntityCreatedEvent(
        Guid? userId,
        string entityType,
        string entityId,
        string newValues,
        string? ipAddress = null,
        string? userAgent = null,
        string? sessionId = null,
        string? requestId = null)
    {
        UserId = userId;
        EntityType = entityType;
        EntityId = entityId;
        NewValues = newValues;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        SessionId = sessionId;
        RequestId = requestId;
    }
}

/// <summary>
/// Domain event published when an entity is updated
/// </summary>
public class EntityUpdatedEvent : BaseDomainEvent
{
    public Guid? UserId { get; }
    public string EntityType { get; }
    public string EntityId { get; }
    public string? OldValues { get; }
    public string? NewValues { get; }
    public string? IpAddress { get; }
    public string? UserAgent { get; }
    public string? SessionId { get; }
    public string? RequestId { get; }

    public EntityUpdatedEvent(
        Guid? userId,
        string entityType,
        string entityId,
        string? oldValues = null,
        string? newValues = null,
        string? ipAddress = null,
        string? userAgent = null,
        string? sessionId = null,
        string? requestId = null)
    {
        UserId = userId;
        EntityType = entityType;
        EntityId = entityId;
        OldValues = oldValues;
        NewValues = newValues;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        SessionId = sessionId;
        RequestId = requestId;
    }
}

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

/// <summary>
/// Domain event published for security-related events
/// </summary>
public class SecurityEvent : BaseDomainEvent
{
    public Guid? UserId { get; }
    public string Action { get; }
    public string EntityType { get; }
    public string EntityId { get; }
    public string? IpAddress { get; }
    public string? UserAgent { get; }
    public string? AdditionalData { get; }
    public string? SessionId { get; }
    public string? RequestId { get; }

    public SecurityEvent(
        Guid? userId,
        string action,
        string entityType,
        string entityId,
        string? ipAddress = null,
        string? userAgent = null,
        string? additionalData = null,
        string? sessionId = null,
        string? requestId = null)
    {
        UserId = userId;
        Action = action;
        EntityType = entityType;
        EntityId = entityId;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        AdditionalData = additionalData;
        SessionId = sessionId;
        RequestId = requestId;
    }
}