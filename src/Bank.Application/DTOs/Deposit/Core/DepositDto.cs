namespace Bank.Application.DTOs.Deposit.Core;

/// <summary>
/// Data transfer object for deposit information
/// </summary>
public class DepositDto
{
    /// <summary>
    /// Deposit ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Unique deposit number
    /// </summary>
    public string DepositNumber { get; set; } = string.Empty;

    /// <summary>
    /// Principal amount deposited
    /// </summary>
    public decimal PrincipalAmount { get; set; }

    /// <summary>
    /// Interest rate applied to the deposit
    /// </summary>
    public decimal InterestRate { get; set; }

    /// <summary>
    /// Date when the deposit matures
    /// </summary>
    public DateTime MaturityDate { get; set; }

    /// <summary>
    /// Current status of the deposit
    /// </summary>
    public string Status { get; set; } = string.Empty;
}
