namespace Bank.Domain.Enums;

/// <summary>
/// Card transaction types
/// </summary>
public enum CardTransactionType
{
    Purchase = 1,
    Withdrawal = 2,
    Refund = 3,
    Fee = 4,
    Interest = 5,
    Payment = 6,
    Transfer = 7,
    Adjustment = 8
}
