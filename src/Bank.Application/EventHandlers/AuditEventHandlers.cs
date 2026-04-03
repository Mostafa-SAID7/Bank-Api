using Bank.Application.Interfaces;
using Bank.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bank.Application.EventHandlers;

/// <summary>
/// Event handler for entity created events to generate audit logs
/// </summary>
public class EntityCreatedEventHandler : INotificationHandler<EntityCreatedEvent>
{
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<EntityCreatedEventHandler> _logger;

    public EntityCreatedEventHandler(
        IAuditLogService auditLogService,
        ILogger<EntityCreatedEventHandler> logger)
    {
        _auditLogService = auditLogService ?? throw new ArgumentNullException(nameof(auditLogService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(EntityCreatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            await _auditLogService.LogUserActionAsync(
                notification.UserId ?? Guid.Empty,
                "CREATE",
                notification.EntityType,
                notification.EntityId,
                newValues: notification.NewValues,
                ipAddress: notification.IpAddress,
                userAgent: notification.UserAgent,
                sessionId: notification.SessionId,
                requestId: notification.RequestId,
                cancellationToken: cancellationToken);

            _logger.LogDebug(
                "Audit log created for entity creation: EntityType={EntityType}, EntityId={EntityId}",
                notification.EntityType, notification.EntityId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to create audit log for entity creation: EntityType={EntityType}, EntityId={EntityId}",
                notification.EntityType, notification.EntityId);
            // Don't rethrow - audit logging should not break business operations
        }
    }
}

/// <summary>
/// Event handler for entity updated events to generate audit logs
/// </summary>
public class EntityUpdatedEventHandler : INotificationHandler<EntityUpdatedEvent>
{
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<EntityUpdatedEventHandler> _logger;

    public EntityUpdatedEventHandler(
        IAuditLogService auditLogService,
        ILogger<EntityUpdatedEventHandler> logger)
    {
        _auditLogService = auditLogService ?? throw new ArgumentNullException(nameof(auditLogService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(EntityUpdatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            await _auditLogService.LogUserActionAsync(
                notification.UserId ?? Guid.Empty,
                "UPDATE",
                notification.EntityType,
                notification.EntityId,
                oldValues: notification.OldValues,
                newValues: notification.NewValues,
                ipAddress: notification.IpAddress,
                userAgent: notification.UserAgent,
                sessionId: notification.SessionId,
                requestId: notification.RequestId,
                cancellationToken: cancellationToken);

            _logger.LogDebug(
                "Audit log created for entity update: EntityType={EntityType}, EntityId={EntityId}",
                notification.EntityType, notification.EntityId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to create audit log for entity update: EntityType={EntityType}, EntityId={EntityId}",
                notification.EntityType, notification.EntityId);
            // Don't rethrow - audit logging should not break business operations
        }
    }
}

/// <summary>
/// Event handler for entity deleted events to generate audit logs
/// </summary>
public class EntityDeletedEventHandler : INotificationHandler<EntityDeletedEvent>
{
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<EntityDeletedEventHandler> _logger;

    public EntityDeletedEventHandler(
        IAuditLogService auditLogService,
        ILogger<EntityDeletedEventHandler> logger)
    {
        _auditLogService = auditLogService ?? throw new ArgumentNullException(nameof(auditLogService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(EntityDeletedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            await _auditLogService.LogUserActionAsync(
                notification.UserId ?? Guid.Empty,
                "DELETE",
                notification.EntityType,
                notification.EntityId,
                oldValues: notification.OldValues,
                ipAddress: notification.IpAddress,
                userAgent: notification.UserAgent,
                sessionId: notification.SessionId,
                requestId: notification.RequestId,
                cancellationToken: cancellationToken);

            _logger.LogDebug(
                "Audit log created for entity deletion: EntityType={EntityType}, EntityId={EntityId}",
                notification.EntityType, notification.EntityId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to create audit log for entity deletion: EntityType={EntityType}, EntityId={EntityId}",
                notification.EntityType, notification.EntityId);
            // Don't rethrow - audit logging should not break business operations
        }
    }
}

/// <summary>
/// Event handler for security events to generate audit logs
/// </summary>
public class SecurityEventHandler : INotificationHandler<SecurityEvent>
{
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<SecurityEventHandler> _logger;

    public SecurityEventHandler(
        IAuditLogService auditLogService,
        ILogger<SecurityEventHandler> logger)
    {
        _auditLogService = auditLogService ?? throw new ArgumentNullException(nameof(auditLogService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(SecurityEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            await _auditLogService.LogSecurityEventAsync(
                notification.UserId,
                notification.Action,
                notification.EntityType,
                notification.EntityId,
                ipAddress: notification.IpAddress,
                userAgent: notification.UserAgent,
                additionalData: notification.AdditionalData,
                sessionId: notification.SessionId,
                requestId: notification.RequestId,
                cancellationToken: cancellationToken);

            _logger.LogWarning(
                "Security event audit log created: Action={Action}, EntityType={EntityType}, EntityId={EntityId}, UserId={UserId}",
                notification.Action, notification.EntityType, notification.EntityId, notification.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to create audit log for security event: Action={Action}, EntityType={EntityType}, EntityId={EntityId}",
                notification.Action, notification.EntityType, notification.EntityId);
            // Don't rethrow - audit logging should not break business operations
        }
    }
}