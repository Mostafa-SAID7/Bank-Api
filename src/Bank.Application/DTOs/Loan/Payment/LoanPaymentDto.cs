namespace Bank.Application.DTOs.Loan.Payment;

/// <summary>
/// Data transfer object for loan payment information
/// </summary>
public class LoanPaymentDto
{
    /// <summary>
    /// Payment ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Associated loan ID
    /// </summary>
    public Guid LoanId { get; set; }

    /// <summary>
    /// Payment amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Date of the payment
    /// </summary>
    public DateTime PaymentDate { get; set; }

    /// <summary>
    /// Payment status
    /// </summary>
    public string Status { get; set; } = string.Empty;
}
