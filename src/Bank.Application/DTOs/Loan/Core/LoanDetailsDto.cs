using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Loan.Core;

/// <summary>
/// DTO for loan details
/// </summary>
public class LoanDto
{
    public Guid Id { get; set; }
    public string LoanNumber { get; set; } = string.Empty;
    public LoanType Type { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int TermInMonths { get; set; }
    public LoanStatus Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public DateTime ApplicationDate { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public DateTime? DisbursementDate { get; set; }
    public DateTime? MaturityDate { get; set; }
    public decimal OutstandingBalance { get; set; }
    public decimal MonthlyPaymentAmount { get; set; }
    public DateTime? NextPaymentDueDate { get; set; }
    public int DaysOverdue { get; set; }
    public decimal TotalInterestPaid { get; set; }
    public decimal TotalPrincipalPaid { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public int? CreditScore { get; set; }
    public CreditScoreRange? CreditScoreRange { get; set; }
}

