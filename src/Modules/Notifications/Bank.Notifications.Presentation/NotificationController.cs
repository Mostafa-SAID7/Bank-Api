using System.Security.Claims;
using Bank.Contracts.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Notifications.Presentation;

/// <summary>
/// Preserves the existing Notifications HTTP surface while delegating all
/// state to the Notifications bounded context.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class NotificationController(
    INotificationDispatchContract dispatcher,
    INotificationManagementContract management) : ControllerBase
{
    [HttpGet("history")]
    public async Task<ActionResult<IReadOnlyList<NotificationHistoryEntry>>> GetNotificationHistory(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized("User not authenticated");
        return Ok(await management.GetHistoryAsync(userId, page, pageSize, cancellationToken));
    }

    [HttpGet("unread-count")]
    public async Task<ActionResult<object>> GetUnreadCount(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized("User not authenticated");
        return Ok(new { unreadCount = await management.GetUnreadCountAsync(userId, cancellationToken) });
    }

    [HttpPost("mark-read/{notificationId:guid}")]
    public async Task<ActionResult<object>> MarkAsRead(Guid notificationId, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized("User not authenticated");
        var updated = await management.MarkAsReadAsync(notificationId, userId, cancellationToken);
        return updated ? Ok(new { success = true, message = "Notification marked as read" }) : NotFound("Notification not found");
    }

    [HttpGet("preferences")]
    public async Task<ActionResult<NotificationPreferencesResponse>> GetPreferences(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized("User not authenticated");
        var preferences = await management.GetPreferencesAsync(userId, cancellationToken) ?? DefaultPreferences();
        return Ok(new NotificationPreferencesResponse(userId.ToString(), preferences));
    }

    [HttpPost("preferences")]
    public async Task<ActionResult<object>> UpdatePreferences(
        [FromBody] UpdateNotificationPreferencesRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized("User not authenticated");
        await management.SavePreferencesAsync(userId, request.ToContract(), cancellationToken);
        return Ok(new { success = true, message = "Notification preferences updated successfully" });
    }

    [HttpPost("test")]
    public async Task<ActionResult<object>> SendTestNotification(
        [FromBody] TestNotificationRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized("User not authenticated");
        var result = await dispatcher.DispatchAsync(new DispatchNotificationRequest(
            userId, "Other", request.Subject ?? "Test Notification", request.Message ?? "This is a test notification",
            request.Channel, NotificationPriority.Normal, Guid.NewGuid().ToString("N")), cancellationToken);
        return Ok(new { notificationId = result.NotificationId.ToString(), success = result.Accepted, message = result.Accepted ? "Notification sent successfully" : result.FailureReason, status = NotificationDeliveryStatus.Pending, sentAt = DateTime.UtcNow });
    }

    private bool TryGetUserId(out Guid userId) => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);

    private static NotificationPreferences DefaultPreferences() => new(
        true, true, true, true, false, 0m, 100m,
        [NotificationChannel.InApp, NotificationChannel.Email], null, null, "en", "UTC");
}

public sealed class TestNotificationRequest
{
    public string? Subject { get; init; }
    public string? Message { get; init; }
    public NotificationChannel Channel { get; init; } = NotificationChannel.InApp;
}

public sealed class UpdateNotificationPreferencesRequest
{
    public string? UserId { get; init; }
    public bool TransactionAlerts { get; init; } = true;
    public bool SecurityAlerts { get; init; } = true;
    public bool LowBalanceAlerts { get; init; } = true;
    public bool PaymentReminders { get; init; } = true;
    public bool MarketingNotifications { get; init; }
    public decimal TransactionAlertThreshold { get; init; }
    public decimal LowBalanceThreshold { get; init; } = 100m;
    public List<NotificationChannel> PreferredChannels { get; init; } = [];
    public string? PhoneNumber { get; init; }
    public string? Email { get; init; }
    public string Language { get; init; } = "en";
    public string TimeZone { get; init; } = "UTC";

    public NotificationPreferences ToContract() => new(
        TransactionAlerts, SecurityAlerts, LowBalanceAlerts, PaymentReminders,
        MarketingNotifications, TransactionAlertThreshold, LowBalanceThreshold,
        PreferredChannels, PhoneNumber, Email, Language, TimeZone);
}

public sealed record NotificationPreferencesResponse(string UserId, NotificationPreferences Preferences)
{
    public bool TransactionAlerts => Preferences.TransactionAlerts;
    public bool SecurityAlerts => Preferences.SecurityAlerts;
    public bool LowBalanceAlerts => Preferences.LowBalanceAlerts;
    public bool PaymentReminders => Preferences.PaymentReminders;
    public bool MarketingNotifications => Preferences.MarketingNotifications;
    public decimal TransactionAlertThreshold => Preferences.TransactionAlertThreshold;
    public decimal LowBalanceThreshold => Preferences.LowBalanceThreshold;
    public IReadOnlyList<NotificationChannel> PreferredChannels => Preferences.PreferredChannels;
    public string? PhoneNumber => Preferences.PhoneNumber;
    public string? Email => Preferences.Email;
    public string Language => Preferences.Language;
    public string TimeZone => Preferences.TimeZone;
}
