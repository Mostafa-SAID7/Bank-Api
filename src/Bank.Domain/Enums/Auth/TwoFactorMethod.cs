namespace Bank.Domain.Enums;

/// <summary>
/// Two-factor authentication methods supported by the system
/// </summary>
public enum TwoFactorMethod
{
    SMS = 1,
    Email = 2,
    AuthenticatorApp = 3,
    BackupCode = 4
}
