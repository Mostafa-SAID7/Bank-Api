namespace Bank.Domain.Enums;

/// <summary>
/// Frequency for compounding interest
/// </summary>
public enum CompoundingFrequency
{
    None = 0,
    Daily = 1,
    Monthly = 2,
    Quarterly = 3,
    SemiAnnually = 4,
    Annually = 5,
    Continuous = 6,
    AtMaturity = 7
}
