namespace Bank.Domain.Enums;

public enum InterestCalculationMethod
{
    Simple = 1,
    Compound = 2,
    DecliningBalance = 3,
    FlatRate = 4,
    ReducingBalance = 5,
    CompoundDaily = 6,
    CompoundMonthly = 7
}
