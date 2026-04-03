namespace Bank.Domain.Enums;

/// <summary>
/// Loan application and processing status
/// </summary>
public enum LoanStatus
{
    UnderReview = 1,
    Approved = 2,
    Rejected = 3,
    Disbursed = 4,
    Active = 5,
    PaidOff = 6,
    Delinquent = 7,
    DefaultStatus = 8,
    Cancelled = 9
}
