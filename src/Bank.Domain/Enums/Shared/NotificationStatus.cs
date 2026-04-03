namespace Bank.Domain.Enums;

/// <summary>
/// Notification status
/// </summary>
public enum NotificationStatus
{
    Pending = 1,
    Sent = 2,
    Delivered = 3,
    Read = 4,
    Failed = 5,
    Cancelled = 6,
    Expired = 7
}
