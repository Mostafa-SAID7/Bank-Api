namespace Bank.Domain.Enums;

/// <summary>
/// Deposit maturity action
/// </summary>
public enum MaturityAction
{
    AutoRenew = 1,
    TransferToPrimary = 2,
    HoldForInstructions = 3,
    PartialRenew = 4,
    Renew = 5,
    Withdraw = 6,
    ReinvestInterest = 7
}

