namespace Bank.Application.DTOs.Payment.BillPayment;

/// <summary>
/// Request DTO for creating a bill payment
/// </summary>
public class CreateBillPaymentRequest
{
    /// <summary>
    /// Biller identification
    /// </summary>
    public string BillerId { get; set; } = string.Empty;

    /// <summary>
    /// Payment amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Reference number for the bill
    /// </summary>
    public string? ReferenceNumber { get; set; }

    /// <summary>
    /// Additional payment details
    /// </summary>
    public string? Details { get; set; }
}
