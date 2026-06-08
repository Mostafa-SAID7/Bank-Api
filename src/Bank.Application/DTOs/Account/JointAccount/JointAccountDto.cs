namespace Bank.Application.DTOs.Account.JointAccount;

/// <summary>
/// Joint account data transfer object
/// </summary>
public class JointAccountDto
{
    /// <summary>
    /// Joint account ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Associated account ID
    /// </summary>
    public Guid AccountId { get; set; }

    /// <summary>
    /// Primary owner ID
    /// </summary>
    public Guid PrimaryOwnerId { get; set; }

    /// <summary>
    /// Secondary owner ID
    /// </summary>
    public Guid SecondaryOwnerId { get; set; }

    /// <summary>
    /// Optional notes about the joint account
    /// </summary>
    public string? Notes { get; set; }
}
