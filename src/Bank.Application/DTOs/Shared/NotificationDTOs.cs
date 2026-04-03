using Bank.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bank.Application.DTOs;

public class NotificationDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public NotificationPriority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

/// <summary>
/// Request to send a notification
/// </summary>
public class SendNotificationRequest
{
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    [Required]
    public NotificationType Type { get; set; }
    
    [Required]
    public string Subject { get; set; } = string.Empty;
    
    [Required]
    public string Message { get; set; } = string.Empty;
    
    public NotificationChannel Channel { get; set; } = NotificationChannel.InApp;
    
    public Dictionary<string, object>? Data { get; set; }
    
    public NotificationPriority Priority { get; set; } = NotificationPriority.Normal;
    
    public DateTime? ScheduledAt { get; set; }
}

/// <summary>
/// Transaction alert request
/// </summary>
public class TransactionAlertRequest
{
    [Required]
    public string TransactionId { get; set; } = string.Empty;
    
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    public string? MerchantName { get; set; }
    
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    
    public bool IsInternational { get; set; }
    
    public bool IsSuspicious { get; set; }
}

/// <summary>
/// Security alert request
/// </summary>
public class SecurityAlertRequest
{
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    [Required]
    public SecurityAlertType AlertType { get; set; }
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    public string? IpAddress { get; set; }
    
    public string? DeviceInfo { get; set; }
    
    public string? Location { get; set; }
    
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Notification preference settings
/// </summary>
public class NotificationPreferencesRequest
{
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    public bool TransactionAlerts { get; set; } = true;
    
    public bool SecurityAlerts { get; set; } = true;
    
    public bool LowBalanceAlerts { get; set; } = true;
    
    public bool PaymentReminders { get; set; } = true;
    
    public bool MarketingNotifications { get; set; } = false;
    
    public decimal TransactionAlertThreshold { get; set; } = 0m;
    
    public decimal LowBalanceThreshold { get; set; } = 100m;
    
    public List<NotificationChannel> PreferredChannels { get; set; } = new();
    
    public string? PhoneNumber { get; set; }
    
    public string? Email { get; set; }
    
    public string Language { get; set; } = "en";
    
    public string TimeZone { get; set; } = "UTC";
}

/// <summary>
/// Notification response
/// </summary>
public class NotificationResponse
{
    public string NotificationId { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public NotificationStatus Status { get; set; }
    public DateTime SentAt { get; set; }
    public List<string> FailedChannels { get; set; } = new();
}

/// <summary>
/// Notification history item
/// </summary>
public class NotificationHistoryItem
{
    public string Id { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationChannel Channel { get; set; }
    public NotificationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// Bulk notification request
/// </summary>
public class BulkNotificationRequest
{
    [Required]
    public List<string> UserIds { get; set; } = new();
    
    [Required]
    public NotificationType Type { get; set; }
    
    [Required]
    public string Subject { get; set; } = string.Empty;
    
    [Required]
    public string Message { get; set; } = string.Empty;
    
    public NotificationChannel Channel { get; set; } = NotificationChannel.InApp;
    
    public NotificationPriority Priority { get; set; } = NotificationPriority.Normal;
    
    public DateTime? ScheduledAt { get; set; }
    
    public Dictionary<string, object>? Data { get; set; }
}