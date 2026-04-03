using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Account.JointAccount;

/// <summary>
/// Request to add a joint holder to an account
/// </summary>
public class AddJointHolderRequest
{
    public int AccessLevel { get; set; }
    public bool CanApproveTransfers { get; set; }
    public bool CanInitiateTransfers { get; set; }
    public DateTime EffectiveDate { get; set; }
    public string JointHolderEmail { get; set; }
    public Guid JointHolderId { get; set; }
    public string JointHolderName { get; set; }
    public string JointHolderPhone { get; set; }
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
    public JointAccountRole Role { get; set; }
    public decimal? TransactionLimit { get; set; }
    public decimal? DailyLimit { get; set; }
    public bool RequiresSignature { get; set; } = true;
    public string? Notes { get; set; }
}


