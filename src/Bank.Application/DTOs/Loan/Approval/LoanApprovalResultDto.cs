using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Loan.Approval;

/// <summary>
/// DTO for loan approval result
/// </summary>
public class LoanApprovalResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public LoanStatus NewStatus { get; set; }
    public decimal? ApprovedAmount { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal? MonthlyPayment { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public List<string> NextSteps { get; set; } = new();
}

