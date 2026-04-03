namespace Bank.Domain.Enums;

/// <summary>
/// Bill payment status
/// </summary>
public enum BillPaymentStatus
{
    Pending = 1,
    Processing = 2,
    Processed = 3,
    Delivered = 4,
    Failed = 5,
    Cancelled = 6,
    Returned = 7
}
