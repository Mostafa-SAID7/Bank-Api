using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Immutable audit log entity for tracking all user actions and system events.
/// Provides comprehensive audit trail for regulatory compliance and security monitoring.
/// </summary>
public class AuditLog : BaseEntity
{
    public DateTime Timestamp { get; set; }
    public Guid? UserId { get; private set; }
    public string Action { get; private set; } = string.Empty;
    public string EntityType { get; private set; } = string.Empty;
    public string EntityId { get; private set; } = string.Empty;
    public string? OldValues { get; private set; }
    public string? NewValues { get; private set; }
    public string? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }
    public AuditEventType EventType { get; private set; }
    public string? AdditionalData { get; private set; }
    public string? SessionId { get; private set; }
    public string? RequestId { get; private set; }

    // Navigation property
    public User? User { get; private set; }

    // Private constructor for EF Core
    private AuditLog() { }

    /// <summary>
    /// Creates a new audit log entry. Once created, audit logs are immutable.
    /// </summary>
    public AuditLog(
        Guid? userId,
        string action,
        string entityType,
        string entityId,
        AuditEventType eventType,
        string? oldValues = null,
        string? newValues = null,
        string? ipAddress = null,
        string? userAgent = null,
        string? additionalData = null,
        string? sessionId = null,
        string? requestId = null)
    {
        UserId = userId;
        Action = action ?? throw new ArgumentNullException(nameof(action));
        EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
        EntityId = entityId ?? throw new ArgumentNullException(nameof(entityId));
        EventType = eventType;
        OldValues = oldValues;
        NewValues = newValues;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        AdditionalData = additionalData;
        SessionId = sessionId;
        RequestId = requestId;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Factory method for creating user action audit logs
    /// </summary>
    public static AuditLog CreateUserAction(
        Guid userId,
        string action,
        string entityType,
        string entityId,
        string? oldValues = null,
        string? newValues = null,
        string? ipAddress = null,
        string? userAgent = null,
        string? sessionId = null,
        string? requestId = null)
    {
        return new AuditLog(
            userId,
            action,
            entityType,
            entityId,
            AuditEventType.UserAction,
            oldValues,
            newValues,
            ipAddress,
            userAgent,
            sessionId: sessionId,
            requestId: requestId);
    }

    /// <summary>
    /// Factory method for creating system event audit logs
    /// </summary>
    public static AuditLog CreateSystemEvent(
        string action,
        string entityType,
        string entityId,
        string? additionalData = null,
        string? requestId = null)
    {
        return new AuditLog(
            null,
            action,
            entityType,
            entityId,
            AuditEventType.SystemEvent,
            additionalData: additionalData,
            requestId: requestId);
    }

    /// <summary>
    /// Factory method for creating security event audit logs
    /// </summary>
    public static AuditLog CreateSecurityEvent(
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
        return new AuditLog(
            userId,
            action,
            entityType,
            entityId,
            AuditEventType.SecurityEvent,
            additionalData: additionalData,
            ipAddress: ipAddress,
            userAgent: userAgent,
            sessionId: sessionId,
            requestId: requestId);
    }
}
