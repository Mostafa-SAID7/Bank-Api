using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Account.JointAccount;

/// <summary>
/// Request to add a joint holder to an account
/// </summary>
public class AddJointHolderRequest
{
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
    public JointAccountRole Role { get; set; }
    public decimal? TransactionLimit { get; set; }
    public decimal? DailyLimit { get; set; }
    public bool RequiresSignature { get; set; } = true;
    public string? Notes { get; set; }
}

