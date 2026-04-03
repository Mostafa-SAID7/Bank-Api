namespace Bank.Domain.Enums;

/// <summary>
/// Loan payment status
/// </summary>
public enum LoanPaymentStatus
{
    Scheduled = 1,
    Paid = 2,
    Overdue = 3,
    Partial = 4,
    Failed = 5,
    Waived = 6
}
