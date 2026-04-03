namespace Bank.Domain.Enums;

/// <summary>
/// Deposit maturity action
/// </summary>
public enum MaturityAction
{
    AutoRenew = 1,
    TransferToPrimary = 2,
    HoldForInstructions = 3,
    PartialRenew = 4
}
