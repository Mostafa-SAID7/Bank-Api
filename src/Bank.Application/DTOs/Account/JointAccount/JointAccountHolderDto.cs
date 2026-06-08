using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Account.JointAccount;

/// <summary>
/// Data transfer object for a joint account holder
/// </summary>
public class JointAccountHolderDto
{
    /// <summary>
    /// Holder ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Account ID
    /// </summary>
    public Guid AccountId { get; set; }

    /// <summary>
    /// User ID
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Username
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// User email
    /// </summary>
    public string UserEmail { get; set; } = string.Empty;

    /// <summary>
    /// Role in the joint account
    /// </summary>
    public JointAccountRole Role { get; set; }

    /// <summary>
    /// Access level
    /// </summary>
    public JointAccountAccessLevel AccessLevel { get; set; }

    /// <summary>
    /// Whether the holder is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Date when holder was added
    /// </summary>
    public DateTime AddedDate { get; set; }

    /// <summary>
    /// Whether holder requires signature for transactions
    /// </summary>
    public bool RequiresSignature { get; set; }

    /// <summary>
    /// Transaction limit for this holder
    /// </summary>
    public decimal? TransactionLimit { get; set; }

    /// <summary>
    /// Daily limit for this holder
    /// </summary>
    public decimal? DailyLimit { get; set; }

    /// <summary>
    /// Optional notes about this holder
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Name of the user who added this holder
    /// </summary>
    public string AddedByUserName { get; set; } = string.Empty;

    /// <summary>
    /// Name of the user who removed this holder (if applicable)
    /// </summary>
    public string? RemovedByUserName { get; set; }

    /// <summary>
    /// Date when this holder was removed (if applicable)
    /// </summary>
    public DateTime? RemovedDate { get; set; }
}