namespace Bank.Domain.Enums;

/// <summary>
/// Bill presentment status
/// </summary>
public enum BillPresentmentStatus
{
    Pending = 1,
    Presented = 2,
    Viewed = 3,
    Paid = 4,
    Overdue = 5,
    Cancelled = 6
}
