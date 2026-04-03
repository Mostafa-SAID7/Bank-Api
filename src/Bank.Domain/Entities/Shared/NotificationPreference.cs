using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents user notification preferences
/// </summary>
public class NotificationPreference : BaseEntity
{
    public bool HasValue { get; set; }
    /// <summary>
    /// User who owns these preferences
    /// </summary>
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    /// <summary>
    /// Enable transaction alerts
    /// </summary>
    public bool TransactionAlerts { get; set; } = true;
    
    /// <summary>
    /// Enable security alerts
    /// </summary>
    public bool SecurityAlerts { get; set; } = true;
    
    /// <summary>
    /// Enable low balance alerts
    /// </summary>
    public bool LowBalanceAlerts { get; set; } = true;
    
    /// <summary>
    /// Enable payment reminders
    /// </summary>
    public bool PaymentReminders { get; set; } = true;
    
    /// <summary>
    /// Enable marketing notifications
    /// </summary>
    public bool MarketingNotifications { get; set; } = false;
    
    /// <summary>
    /// Enable card alerts
    /// </summary>
    public bool CardAlerts { get; set; } = true;
    
    /// <summary>
    /// Enable loan alerts
    /// </summary>
    public bool LoanAlerts { get; set; } = true;
    
    /// <summary>
    /// Enable account update notifications
    /// </summary>
    public bool AccountUpdates { get; set; } = true;
    
    /// <summary>
    /// Minimum transaction amount to trigger alert
    /// </summary>
    public decimal TransactionAlertThreshold { get; set; } = 0m;
    
    /// <summary>
    /// Low balance threshold amount
    /// </summary>
    public decimal LowBalanceThreshold { get; set; } = 100m;
    
    /// <summary>
    /// Preferred notification channels (JSON array)
    /// </summary>
    public string PreferredChannels { get; set; } = "[1,2]"; // InApp, Email by default
    
    /// <summary>
    /// Phone number for SMS notifications
    /// </summary>
    public string? PhoneNumber { get; set; }
    
    /// <summary>
    /// Email address for email notifications
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// Preferred language for notifications
    /// </summary>
    public string Language { get; set; } = "en";
    
    /// <summary>
    /// User's timezone
    /// </summary>
    public string TimeZone { get; set; } = "UTC";
    
    /// <summary>
    /// Quiet hours start time (24-hour format)
    /// </summary>
    public TimeSpan? QuietHoursStart { get; set; }
    
    /// <summary>
    /// Quiet hours end time (24-hour format)
    /// </summary>
    public TimeSpan? QuietHoursEnd { get; set; }
    
    /// <summary>
    /// Enable notifications during quiet hours for critical alerts
    /// </summary>
    public bool AllowCriticalDuringQuietHours { get; set; } = true;

    // Domain methods
    
    /// <summary>
    /// Check if notification type is enabled
    /// </summary>
    public bool IsNotificationTypeEnabled(NotificationType type)
    {
        return type switch
        {
            NotificationType.TransactionAlert => TransactionAlerts,
            NotificationType.SecurityAlert => SecurityAlerts,
            NotificationType.LowBalance => LowBalanceAlerts,
            NotificationType.PaymentReminder => PaymentReminders,
            NotificationType.Marketing => MarketingNotifications,
            NotificationType.CardAlert => CardAlerts,
            NotificationType.LoanAlert => LoanAlerts,
            NotificationType.AccountUpdate => AccountUpdates,
            _ => true // Default to enabled for other types
        };
    }
    
    /// <summary>
    /// Check if transaction amount meets alert threshold
    /// </summary>
    public bool ShouldAlertForTransaction(decimal amount)
    {
        return TransactionAlerts && amount >= TransactionAlertThreshold;
    }
    
    /// <summary>
    /// Check if balance meets low balance alert threshold
    /// </summary>
    public bool ShouldAlertForLowBalance(decimal balance)
    {
        return LowBalanceAlerts && balance <= LowBalanceThreshold;
    }
    
    /// <summary>
    /// Get preferred notification channels
    /// </summary>
    public List<NotificationChannel> GetPreferredChannels()
    {
        try
        {
            var channelIds = System.Text.Json.JsonSerializer.Deserialize<List<int>>(PreferredChannels);
            return channelIds?.Cast<NotificationChannel>().ToList() ?? new List<NotificationChannel> { NotificationChannel.InApp };
        }
        catch
        {
            return new List<NotificationChannel> { NotificationChannel.InApp };
        }
    }
    
    /// <summary>
    /// Set preferred notification channels
    /// </summary>
    public void SetPreferredChannels(List<NotificationChannel> channels)
    {
        var channelIds = channels.Cast<int>().ToList();
        PreferredChannels = System.Text.Json.JsonSerializer.Serialize(channelIds);
    }
    
    /// <summary>
    /// Check if current time is within quiet hours
    /// </summary>
    public bool IsInQuietHours(DateTime dateTime)
    {
        if (!QuietHoursStart.HasValue || !QuietHoursEnd.HasValue)
            return false;
            
        var currentTime = dateTime.TimeOfDay;
        var start = QuietHoursStart.Value;
        var end = QuietHoursEnd.Value;
        
        // Handle quiet hours that span midnight
        if (start > end)
        {
            return currentTime >= start || currentTime <= end;
        }
        
        return currentTime >= start && currentTime <= end;
    }
    
    /// <summary>
    /// Check if notification should be sent during quiet hours
    /// </summary>
    public bool ShouldSendDuringQuietHours(NotificationPriority priority)
    {
        return !IsInQuietHours(DateTime.UtcNow) || 
               (AllowCriticalDuringQuietHours && priority == NotificationPriority.Critical);
    }
}
