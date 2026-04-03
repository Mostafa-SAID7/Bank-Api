namespace Bank.Domain.Enums;

/// <summary>
/// Two-factor authentication setup status
/// </summary>
public enum TwoFactorStatus
{
    NotSetup = 0,
    Pending = 1,
    Active = 2,
    Disabled = 3
}
