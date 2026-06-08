using Bank.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bank.Application.DTOs.Loan.Core;

/// <summary>
/// Request DTO for creating a new loan
/// </summary>
public class CreateLoanRequest
{
    /// <summary>
    /// Type of loan
    /// </summary>
    [Required]
    public LoanType Type { get; set; }

    /// <summary>
    /// Requested loan amount
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal RequestedAmount { get; set; }

    /// <summary>
    /// Term in months
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    public int TermInMonths { get; set; }

    /// <summary>
    /// Purpose of the loan
    /// </summary>
    [Required]
    public string Purpose { get; set; } = string.Empty;
}
