using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Account.Core;

/// <summary>
/// Account data transfer object
/// </summary>
public class AccountDto
{
    public Guid Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountHolderName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public AccountStatus Status { get; set; }
    public AccountType Type { get; set; }
    public DateTime OpenedDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public string? ClosureReason { get; set; }
    public DateTime LastActivityDate { get; set; }
    public DateTime? DormancyDate { get; set; }
    public decimal MinimumBalance { get; set; }
    public decimal MonthlyMaintenanceFee { get; set; }
    public bool FeeWaiverEligible { get; set; }
    public decimal InterestRate { get; set; }
    public DateTime? LastInterestCalculationDate { get; set; }
    public InterestCompoundingFrequency CompoundingFrequency { get; set; }
    public bool IsJointAccount { get; set; }
    public bool RequiresMultipleSignatures { get; set; }
    public decimal? MultipleSignatureThreshold { get; set; }
    public int MinimumSignaturesRequired { get; set; }
    public bool HasHolds { get; set; }
    public bool HasRestrictions { get; set; }
}
