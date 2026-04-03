using Bank.Domain.Common;

namespace Bank.Domain.Events;

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
