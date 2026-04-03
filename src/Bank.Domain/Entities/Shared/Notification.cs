using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents a notification sent to a user
/// </summary>
public class Notification : BaseEntity
{
    /// <summary>
    /// User who receives this notification
    /// </summary>
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    /// <summary>
    /// Type of notification
    /// </summary>
    public NotificationType Type { get; set; }
    
    /// <summary>
    /// Notification subject/title
    /// </summary>
    public string Subject { get; set; } = string.Empty;
    
    /// <summary>
    /// Notification message content
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// Channel used to send notification
    /// </summary>
    public NotificationChannel Channel { get; set; }
    
    /// <summary>
    /// Current status of notification
    /// </summary>
    public NotificationStatus Status { get; set; } = NotificationStatus.Pending;
    
    /// <summary>
    /// Priority level of notification
    /// </summary>
    public NotificationPriority Priority { get; set; } = NotificationPriority.Normal;
    
    /// <summary>
    /// When notification was sent
    /// </summary>
    public DateTime? SentAt { get; set; }
    
    /// <summary>
    /// When notification was read by user
    /// </summary>
    public DateTime? ReadAt { get; set; }
    
    /// <summary>
    /// When notification should be sent (for scheduled notifications)
    /// </summary>
    public DateTime? ScheduledAt { get; set; }
    
    /// <summary>
    /// When notification expires
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
    
    /// <summary>
    /// Additional data as JSON
    /// </summary>
    public string? Data { get; set; }
    
    /// <summary>
    /// Error message if notification failed
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// Number of retry attempts
    /// </summary>
    public int RetryCount { get; set; } = 0;
    
    /// <summary>
    /// Maximum retry attempts allowed
    /// </summary>
    public int MaxRetries { get; set; } = 3;
    
    /// <summary>
    /// External reference ID (e.g., from SMS provider)
    /// </summary>
    public string? ExternalReferenceId { get; set; }
    
    /// <summary>
    /// Template used for this notification
    /// </summary>
    public string? TemplateId { get; set; }
    
    /// <summary>
    /// Language code for the notification
    /// </summary>
    public string Language { get; set; } = "en";

    // Domain methods
    
    /// <summary>
    /// Mark notification as sent
    /// </summary>
    public void MarkAsSent(string? externalReferenceId = null)
    {
        Status = NotificationStatus.Sent;
        SentAt = DateTime.UtcNow;
        ExternalReferenceId = externalReferenceId;
    }
    
    /// <summary>
    /// Mark notification as delivered
    /// </summary>
    public void MarkAsDelivered()
    {
        if (Status == NotificationStatus.Sent)
        {
            Status = NotificationStatus.Delivered;
        }
    }
    
    /// <summary>
    /// Mark notification as read
    /// </summary>
    public void MarkAsRead()
    {
        if (Status == NotificationStatus.Delivered || Status == NotificationStatus.Sent)
        {
            Status = NotificationStatus.Read;
            ReadAt = DateTime.UtcNow;
        }
    }
    
    /// <summary>
    /// Mark notification as failed
    /// </summary>
    public void MarkAsFailed(string errorMessage)
    {
        Status = NotificationStatus.Failed;
        ErrorMessage = errorMessage;
        RetryCount++;
    }
    
    /// <summary>
    /// Check if notification can be retried
    /// </summary>
    public bool CanRetry()
    {
        return Status == NotificationStatus.Failed && RetryCount < MaxRetries;
    }
    
    /// <summary>
    /// Check if notification is expired
    /// </summary>
    public bool IsExpired()
    {
        return ExpiresAt.HasValue && ExpiresAt.Value <= DateTime.UtcNow;
    }
    
    /// <summary>
    /// Check if notification is ready to send
    /// </summary>
    public bool IsReadyToSend()
    {
        return Status == NotificationStatus.Pending && 
               (!ScheduledAt.HasValue || ScheduledAt.Value <= DateTime.UtcNow) &&
               !IsExpired();
    }
}