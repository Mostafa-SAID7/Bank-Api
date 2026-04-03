using Bank.Application.Interfaces;
using Bank.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bank.Api.Controllers;

/// <summary>
/// Controller for audit log operations.
/// Provides read-only access to audit logs for compliance and security analysis.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuditController : ControllerBase
{
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<AuditController> _logger;

    public AuditController(IAuditLogService auditLogService, ILogger<AuditController> logger)
    {
        _auditLogService = auditLogService ?? throw new ArgumentNullException(nameof(auditLogService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets audit logs for the current user
    /// </summary>
    [HttpGet("my-logs")]
    public async Task<IActionResult> GetMyAuditLogs(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid user ID");
            }

            var auditLogs = await _auditLogService.GetUserAuditLogsAsync(
                userId, fromDate, toDate, pageNumber, pageSize);

            return Ok(auditLogs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user audit logs");
            return StatusCode(500, "An error occurred while retrieving audit logs");
        }
    }

    /// <summary>
    /// Gets audit logs for a specific user (admin only)
    /// </summary>
    [HttpGet("user/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserAuditLogs(
        Guid userId,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        try
        {
            var auditLogs = await _auditLogService.GetUserAuditLogsAsync(
                userId, fromDate, toDate, pageNumber, pageSize);

            return Ok(auditLogs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user audit logs for user {UserId}", userId);
            return StatusCode(500, "An error occurred while retrieving audit logs");
        }
    }

    /// <summary>
    /// Gets audit logs for a specific entity (admin only)
    /// </summary>
    [HttpGet("entity/{entityType}/{entityId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetEntityAuditLogs(
        string entityType,
        string entityId,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        try
        {
            var auditLogs = await _auditLogService.GetEntityAuditLogsAsync(
                entityType, entityId, fromDate, toDate, pageNumber, pageSize);

            return Ok(auditLogs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving entity audit logs for {EntityType}:{EntityId}", entityType, entityId);
            return StatusCode(500, "An error occurred while retrieving audit logs");
        }
    }

    /// <summary>
    /// Gets audit logs by event type (admin only)
    /// </summary>
    [HttpGet("event-type/{eventType}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAuditLogsByEventType(
        AuditEventType eventType,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        try
        {
            var auditLogs = await _auditLogService.GetAuditLogsByEventTypeAsync(
                eventType, fromDate, toDate, pageNumber, pageSize);

            return Ok(auditLogs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving audit logs by event type {EventType}", eventType);
            return StatusCode(500, "An error occurred while retrieving audit logs");
        }
    }

    /// <summary>
    /// Gets audit logs by action (admin only)
    /// </summary>
    [HttpGet("action/{action}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAuditLogsByAction(
        string action,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        try
        {
            var auditLogs = await _auditLogService.GetAuditLogsByActionAsync(
                action, fromDate, toDate, pageNumber, pageSize);

            return Ok(auditLogs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving audit logs by action {Action}", action);
            return StatusCode(500, "An error occurred while retrieving audit logs");
        }
    }

    /// <summary>
    /// Gets audit logs by IP address for security analysis (admin only)
    /// </summary>
    [HttpGet("ip/{ipAddress}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAuditLogsByIpAddress(
        string ipAddress,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        try
        {
            var auditLogs = await _auditLogService.GetAuditLogsByIpAddressAsync(
                ipAddress, fromDate, toDate, pageNumber, pageSize);

            return Ok(auditLogs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving audit logs by IP address {IpAddress}", ipAddress);
            return StatusCode(500, "An error occurred while retrieving audit logs");
        }
    }

    /// <summary>
    /// Gets audit log statistics for reporting (admin only)
    /// </summary>
    [HttpGet("statistics")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAuditLogStatistics(
        [FromQuery] DateTime fromDate,
        [FromQuery] DateTime toDate)
    {
        try
        {
            if (fromDate == default || toDate == default)
            {
                return BadRequest("Both fromDate and toDate are required");
            }

            if (fromDate > toDate)
            {
                return BadRequest("fromDate cannot be greater than toDate");
            }

            var statistics = await _auditLogService.GetAuditLogStatisticsAsync(fromDate, toDate);

            return Ok(statistics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving audit log statistics");
            return StatusCode(500, "An error occurred while retrieving audit log statistics");
        }
    }
}