namespace Bank.Domain.Enums;

/// <summary>
/// Card block reasons
/// </summary>
public enum CardBlockReason
{
    CustomerRequest = 1,
    LostCard = 2,
    StolenCard = 3,
    SuspiciousActivity = 4,
    ExcessiveDeclines = 5,
    OverLimit = 6,
    Expired = 7,
    DamagedCard = 8,
    ComplianceHold = 9,
    SystemSecurity = 10
}
