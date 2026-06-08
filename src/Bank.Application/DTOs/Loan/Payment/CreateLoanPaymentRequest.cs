using System.ComponentModel.DataAnnotations;

namespace Bank.Application.DTOs.Loan.Payment;

/// <summary>
/// Request DTO for creating a loan payment
/// </summary>
public class CreateLoanPaymentRequest
{
    /// <summary>
    /// Loan ID to make payment for
    /// </summary>
    [Required]
    public Guid LoanId { get; set; }

    /// <summary>
    /// Payment amount
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    /// <summary>
    /// Payment reference number (optional)
    /// </summary>
    public string? ReferenceNumber { get; set; }
}
