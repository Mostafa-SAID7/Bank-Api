namespace Bank.Application.DTOs.Account.JointAccount;

/// <summary>
/// Request to create a joint account
/// </summary>
public class CreateJointAccountRequest
{
    /// <summary>
    /// Primary account ID
    /// </summary>
    public Guid AccountId { get; set; }

    /// <summary>
    /// Secondary owner user ID
    /// </summary>
    public Guid SecondaryOwnerId { get; set; }
}
