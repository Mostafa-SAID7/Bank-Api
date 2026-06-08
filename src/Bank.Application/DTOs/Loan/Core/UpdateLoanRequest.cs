namespace Bank.Application.DTOs.Loan.Core;

/// <summary>
/// Request DTO for updating loan information
/// </summary>
public class UpdateLoanRequest
{
    /// <summary>
    /// Loan ID to update
    /// </summary>
    public Guid LoanId { get; set; }

    /// <summary>
    /// Updated purpose of the loan
    /// </summary>
    public string? Purpose { get; set; }

    /// <summary>
    /// Additional notes about the loan
    /// </summary>
    public string? Notes { get; set; }
}
