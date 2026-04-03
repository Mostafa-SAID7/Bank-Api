namespace Bank.Domain.Enums;

/// <summary>
/// Interest calculation methods
/// </summary>
public enum InterestCalculationMethod
{
    Simple = 1,
    CompoundMonthly = 2,
    CompoundDaily = 3,
    ReducingBalance = 4,
    FlatRate = 5
}
