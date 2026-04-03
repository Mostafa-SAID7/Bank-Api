using Bank.Application.Interfaces;
using Bank.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bank.Application.EventHandlers;

/// <summary>
/// Event handler for entity updated events to generate audit logs
/// </summary>
public class AuditLogUpdatedEventHandler : INotificationHandler<EntityUpdatedEvent>
{
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<AuditLogUpdatedEventHandler> _logger;

    public AuditLogUpdatedEventHandler(
        IAuditLogService auditLogService,
        ILogger<AuditLogUpdatedEventHandler> logger)
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
