namespace Bank.Domain.Enums;

/// <summary>
/// Types of maturity notices
/// </summary>
public enum MaturityNoticeType
{
    Initial = 1,
    Reminder = 2,
    Final = 3,
    AutoRenewal = 4,
    MaturityConfirmation = 5
}
