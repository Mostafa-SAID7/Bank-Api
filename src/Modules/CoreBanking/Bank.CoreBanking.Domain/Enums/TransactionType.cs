namespace Bank.CoreBanking.Domain.Enums;

public enum TransactionType
{
    Transfer = 1,
    Deposit = 2,
    Withdrawal = 3,
    Payment = 4,
    Fee = 5,
    Interest = 6,
    Reversal = 7,
    Other = 8
}
