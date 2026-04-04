using Bank.Domain.Common;

namespace Bank.Domain.Entities;

/// <summary>
/// Dummy entity to resolve JointAccount reference in mapping profiles while the account system is being restored
/// </summary>
public class JointAccount : BaseEntity
{
    public Guid AccountId { get; set; }
    public Guid PrimaryOwnerId { get; set; }
    public Guid SecondaryOwnerId { get; set; }
    public string? Notes { get; set; }
}
