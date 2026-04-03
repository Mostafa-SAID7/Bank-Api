using Bank.Application.Interfaces;
using Bank.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bank.Application.EventHandlers;

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
