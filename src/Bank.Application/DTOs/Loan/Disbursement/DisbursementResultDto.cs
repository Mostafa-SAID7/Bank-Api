namespace Bank.Application.DTOs.Loan.Disbursement;

/// <summary>
/// DTO for loan disbursement result
/// </summary>
public class DisbursementResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public decimal DisbursedAmount { get; set; }
    public string? TransactionReference { get; set; }
    public DateTime DisbursementDate { get; set; }
    public DateTime FirstPaymentDueDate { get; set; }
    public decimal MonthlyPaymentAmount { get; set; }
}

