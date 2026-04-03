namespace Bank.Domain.Enums;

/// <summary>
/// Notification types
/// </summary>
public enum NotificationType
{
    TransactionAlert = 1,
    SecurityAlert = 2,
    LowBalance = 3,
    PaymentReminder = 4,
    AccountUpdate = 5,
    CardAlert = 6,
    LoanAlert = 7,
    SystemMaintenance = 8,
    Marketing = 9,
    Compliance = 10,
    Welcome = 11,
    PasswordExpiry = 12,
    LoginAlert = 13,
    Other = 99
}
