namespace Bank.Domain.Enums;

public enum AccountRestrictionType
{
    NoDebits = 1,
    NoCredits = 2,
    NoTransfers = 3,
    LimitedAccess = 4,
    ReadOnly = 5
}
