namespace Bank.Domain.Enums;

/// <summary>
/// Card transaction status
/// </summary>
public enum CardTransactionStatus
{
    Pending = 1,
    Authorized = 2,
    Settled = 3,
    Declined = 4,
    Reversed = 5,
    Disputed = 6
}
