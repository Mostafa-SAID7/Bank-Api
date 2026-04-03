namespace Bank.Domain.Enums;

/// <summary>
/// Types of deposit transactions
/// </summary>
public enum DepositTransactionType
{
    InterestCredit = 1,
    PenaltyCharge = 2,
    MaturityPayout = 3,
    PartialWithdrawal = 4,
    EarlyWithdrawal = 5,
    RenewalCredit = 6,
    FeeCharge = 7,
    Adjustment = 8,
    Reversal = 9
}
