using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Account.JointAccount;

/// <summary>
/// Joint account holder information
/// </summary>
public class JointAccountHolderDto
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public JointAccountRole Role { get; set; }
    public DateTime AddedDate { get; set; }
    public DateTime? RemovedDate { get; set; }
    public bool IsActive { get; set; }
    public bool RequiresSignature { get; set; }
    public decimal? TransactionLimit { get; set; }
    public decimal? DailyLimit { get; set; }
    public string? Notes { get; set; }
    public string AddedByUserName { get; set; } = string.Empty;
    public string? RemovedByUserName { get; set; }
}

