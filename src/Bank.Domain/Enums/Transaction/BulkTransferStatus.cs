namespace Bank.Domain.Enums;

public enum BulkTransferStatus
{
    Pending = 1,
    Processing = 2,
    Completed = 3,
    Failed = 4,
    PartiallyCompleted = 5
}
