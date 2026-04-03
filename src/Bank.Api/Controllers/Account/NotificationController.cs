using Bank.Api.Helpers;
using Bank.Application.DTOs;
using Bank.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers.Account;

/// <summary>
/// Controller for managing notifications
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(
        INotificationService notificationService,
        ILogger<NotificationController> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    /// <summary>
    /// Get notification history for the current user
    /// </summary>
    [HttpGet("history")]
    public async Task<ActionResult<List<NotificationHistoryItem>>> GetNotificationHistory(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 20)
    {
        try
        {
            var userId = this.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            var notifications = await _notificationService.GetNotificationHistoryAsync(userId, page, pageSize);
            return Ok(notifications);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting notification history");
            return StatusCode(500, "An error occurred while retrieving notification history");
        }
    }

    /// <summary>
    /// Get unread notification count for the current user
    /// </summary>
    [HttpGet("unread-count")]
    public async Task<ActionResult<object>> GetUnreadCount()
    {
        try
        {
            var userId = this.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            var count = await _notificationService.GetUnreadCountAsync(userId);
            return Ok(new { unreadCount = count });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting unread notification count");
            return StatusCode(500, "An error occurred while retrieving unread count");
        }
    }

    /// <summary>
    /// Mark a notification as read
    /// </summary>
    [HttpPost("mark-read/{notificationId}")]
    public async Task<ActionResult<object>> MarkAsRead(string notificationId)
    {
        try
        {
            var userId = this.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            var success = await _notificationService.MarkAsReadAsync(notificationId, userId);
            
            if (!success)
            {
                return NotFound("Notification not found");
            }

            return Ok(new { success = true, message = "Notification marked as read" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking notification as read");
            return StatusCode(500, "An error occurred while marking notification as read");
        }
    }

    /// <summary>
    /// Get notification preferences for the current user
    /// </summary>
    [HttpGet("preferences")]
    public async Task<ActionResult<NotificationPreferencesRequest>> GetPreferences()
    {
        try
        {
            var userId = this.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            var preferences = await _notificationService.GetPreferencesAsync(userId);
            
            if (preferences == null)
            {
                // Return default preferences
                return Ok(new NotificationPreferencesRequest
                {
                    UserId = userId,
                    TransactionAlerts = true,
                    SecurityAlerts = true,
                    LowBalanceAlerts = true,
                    PaymentReminders = true,
                    MarketingNotifications = false,
                    TransactionAlertThreshold = 0m,
                    LowBalanceThreshold = 100m,
                    PreferredChannels = new List<Domain.Enums.NotificationChannel> 
                    { 
                        Domain.Enums.NotificationChannel.InApp, 
                        Domain.Enums.NotificationChannel.Email 
                    },
                    Language = "en",
                    TimeZone = "UTC"
                });
            }

            return Ok(preferences);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting notification preferences");
            return StatusCode(500, "An error occurred while retrieving notification preferences");
        }
    }

    /// <summary>
    /// Update notification preferences for the current user
    /// </summary>
    [HttpPost("preferences")]
    public async Task<ActionResult<object>> UpdatePreferences([FromBody] NotificationPreferencesRequest request)
    {
        try
        {
            var userId = this.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            // Ensure the request is for the current user
            request.UserId = userId;

            var success = await _notificationService.UpdatePreferencesAsync(request);
            
            if (!success)
            {
                return BadRequest("Failed to update notification preferences");
            }

            return Ok(new { success = true, message = "Notification preferences updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating notification preferences");
            return StatusCode(500, "An error occurred while updating notification preferences");
        }
    }

    /// <summary>
    /// Send a test notification (for testing purposes)
    /// </summary>
    [HttpPost("test")]
    public async Task<ActionResult<NotificationResponse>> SendTestNotification([FromBody] TestNotificationRequest request)
    {
        try
        {
            var userId = this.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            var notificationRequest = new SendNotificationRequest
            {
                UserId = userId,
                Type = Domain.Enums.NotificationType.Other,
                Subject = request.Subject ?? "Test Notification",
                Message = request.Message ?? "This is a test notification",
                Channel = request.Channel,
                Priority = Domain.Enums.NotificationPriority.Normal
            };

            var result = await _notificationService.SendNotificationAsync(notificationRequest);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending test notification");
            return StatusCode(500, "An error occurred while sending test notification");
        }
    }
}

/// <summary>
/// Request to send a test notification
/// </summary>
public class TestNotificationRequest
{
    public string? Subject { get; set; }
    public string? Message { get; set; }
    public Domain.Enums.NotificationChannel Channel { get; set; } = Domain.Enums.NotificationChannel.InApp;
}