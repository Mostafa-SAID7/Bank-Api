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
