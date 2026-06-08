using System.ComponentModel.DataAnnotations;

namespace Bank.Application.DTOs.Deposit.Core;

/// <summary>
/// Request DTO for creating a new deposit
/// </summary>
public class CreateDepositRequest
{
    /// <summary>
    /// Account ID to create deposit for
    /// </summary>
    [Required]
    public Guid AccountId { get; set; }

    /// <summary>
    /// Deposit product ID
    /// </summary>
    [Required]
    public Guid DepositProductId { get; set; }

    /// <summary>
    /// Principal amount to deposit
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal PrincipalAmount { get; set; }

    /// <summary>
    /// Term in days (optional - uses product default if not specified)
    /// </summary>
    public int? TermDays { get; set; }
}
