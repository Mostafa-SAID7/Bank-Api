using Bank.Application.Interfaces;
using System.Security.Claims;
using System.Text.Json;

namespace Bank.Api.Middleware.Audit;

/// <summary>
/// Handles logging of audit entries and security events
/// </summary>
public class AuditLogger
{
    private readonly ILogger<AuditLogger> _logger;

    public AuditLogger(ILogger<AuditLogger> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task LogAuditEntryAsync(
        HttpContext context, 
        string requestDetails, 
        string responseDetails, 
        long durationMs,
        string requestId)
    {
        var auditService = context.RequestServices.GetService<IAuditLogService>();
        if (auditService == null) return;

        var userId = GetUserId(context);
        var ipAddress = GetClientIpAddress(context);
        var userAgent = context.Request.Headers["User-Agent"].FirstOrDefault();
        var sessionId = context.Session?.Id;

        var action = $"{context.Request.Method} {context.Request.Path}";
        var additionalData = JsonSerializer.Serialize(new
        {
            Request = requestDetails,
            Response = responseDetails,
            DurationMs = durationMs,
            StatusCode = context.Response.StatusCode
        });

        if (context.Response.StatusCode >= 400)
        {
            // Log as security event for error responses
            await auditService.LogSecurityEventAsync(
                userId,
                action,
                "HTTP_REQUEST",
                requestId,
                ipAddress,
                userAgent,
                additionalData,
                sessionId,
                requestId);
        }
        else
        {
            // Log as user action for successful requests
            await auditService.LogUserActionAsync(
                userId ?? Guid.Empty,
                action,
                "HTTP_REQUEST",
                requestId,
                requestDetails,
                responseDetails,
                ipAddress,
                userAgent,
                sessionId,
                requestId);
        }
    }

    public async Task LogSecurityEventAsync(HttpContext context, string action, string additionalData, string requestId)
    {
        var auditService = context.RequestServices.GetService<IAuditLogService>();
        if (auditService == null) return;

        var userId = GetUserId(context);
        var ipAddress = GetClientIpAddress(context);
        var userAgent = context.Request.Headers["User-Agent"].FirstOrDefault();
        var sessionId = context.Session?.Id;

        await auditService.LogSecurityEventAsync(
            userId,
            action,
            "SECURITY_EVENT",
            requestId,
            ipAddress,
            userAgent,
            additionalData,
            sessionId,
            requestId);
    }

    private Guid? GetUserId(HttpContext context)
    {
        var userIdClaim = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    private string? GetClientIpAddress(HttpContext context)
    {
        // Check for forwarded IP first (load balancer/proxy scenarios)
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            return forwardedFor.Split(',')[0].Trim();
        }

        var realIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(realIp))
        {
            return realIp;
        }

        return context.Connection.RemoteIpAddress?.ToString();
    }
}
