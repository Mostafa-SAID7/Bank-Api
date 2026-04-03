namespace Bank.Domain.Enums;

/// <summary>
/// Interest tier calculation basis
/// </summary>
public enum InterestTierBasis
{
    Balance = 1,
    Term = 2,
    Relationship = 3,
    Promotional = 4
}
