using Bank.Application.Interfaces;
using Bank.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bank.Application.EventHandlers;

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
