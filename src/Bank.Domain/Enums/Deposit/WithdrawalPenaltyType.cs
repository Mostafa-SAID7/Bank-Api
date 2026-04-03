namespace Bank.Domain.Enums;

/// <summary>
/// Deposit withdrawal penalty type
/// </summary>
public enum WithdrawalPenaltyType
{
    None = 1,
    FixedAmount = 2,
    Percentage = 3,
    InterestForfeiture = 4,
    Combined = 5
}
