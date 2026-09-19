namespace Bank.CoreBanking.Domain.Enums;

public enum TransactionStatus
{
    Pending = 1,
    Posted = 2,
    Completed = 3,
    Failed = 4,
    Reversed = 5
}
