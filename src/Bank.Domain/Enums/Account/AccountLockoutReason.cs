namespace Bank.Domain.Enums;

public enum AccountLockoutReason
{
    FailedLoginAttempts = 1,
    SuspiciousActivity = 2,
    AdminAction = 3,
    ComplianceHold = 4
}
