namespace Bank.Domain.Enums;

/// <summary>
/// Security alert types
/// </summary>
public enum SecurityAlertType
{
    LoginFromNewDevice = 1,
    LoginFromNewLocation = 2,
    PasswordChanged = 3,
    TwoFactorDisabled = 4,
    AccountLocked = 5,
    SuspiciousActivity = 6,
    UnauthorizedAccess = 7,
    DataBreach = 8,
    PhishingAttempt = 9,
    MalwareDetected = 10,
    CompromisedCredentials = 11,
    Other = 99
}
