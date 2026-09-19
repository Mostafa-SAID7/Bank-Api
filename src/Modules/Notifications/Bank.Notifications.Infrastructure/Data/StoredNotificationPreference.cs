namespace Bank.Notifications.Infrastructure.Data;

/// <summary>
/// Notifications-owned user settings. UserId is an external identifier; the
/// module deliberately has no foreign key to Identity's tables.
/// </summary>
internal sealed class StoredNotificationPreference
{
    public Guid UserId { get; set; }
    public bool TransactionAlerts { get; set; } = true;
    public bool SecurityAlerts { get; set; } = true;
    public bool LowBalanceAlerts { get; set; } = true;
    public bool PaymentReminders { get; set; } = true;
    public bool MarketingNotifications { get; set; }
    public decimal TransactionAlertThreshold { get; set; }
    public decimal LowBalanceThreshold { get; set; } = 100m;
    public string PreferredChannels { get; set; } = "[1,2]";
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string Language { get; set; } = "en";
    public string TimeZone { get; set; } = "UTC";
}
