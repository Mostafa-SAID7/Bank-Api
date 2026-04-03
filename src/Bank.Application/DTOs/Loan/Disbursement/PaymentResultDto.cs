namespace Bank.Application.DTOs.Loan.Disbursement;

/// <summary>
/// DTO for loan payment result
/// </summary>
public class PaymentResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public decimal PaymentAmount { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestAmount { get; set; }
    public decimal RemainingBalance { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? TransactionReference { get; set; }
    public DateTime? NextPaymentDueDate { get; set; }
}

