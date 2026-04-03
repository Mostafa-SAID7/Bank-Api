using Bank.Domain.Enums;

namespace Bank.Application.DTOs;

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

/// <summary>
/// Request to remove a joint holder from an account
/// </summary>
public class RemoveJointHolderRequest
{
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
    public string? Reason { get; set; }
}

/// <summary>
/// Request to update joint holder role and permissions
/// </summary>
public class UpdateJointHolderRequest
{
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
    public JointAccountRole NewRole { get; set; }
    public decimal? TransactionLimit { get; set; }
    public decimal? DailyLimit { get; set; }
    public bool RequiresSignature { get; set; }
    public string? Notes { get; set; }
}

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

/// <summary>
/// Request to convert account to joint account
/// </summary>
public class ConvertToJointAccountRequest
{
    public Guid AccountId { get; set; }
    public bool RequiresMultipleSignatures { get; set; } = false;
    public decimal? MultipleSignatureThreshold { get; set; }
    public int MinimumSignaturesRequired { get; set; } = 2;
}

/// <summary>
/// Request to convert joint account to single account
/// </summary>
public class ConvertToSingleAccountRequest
{
    public Guid AccountId { get; set; }
    public Guid RemainingHolderId { get; set; }
    public string? Reason { get; set; }
}

/// <summary>
/// Transaction permission check request
/// </summary>
public class TransactionPermissionRequest
{
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
}

/// <summary>
/// Transaction permission result
/// </summary>
public class TransactionPermissionResult
{
    public bool CanPerformTransaction { get; set; }
    public bool RequiresMultipleSignatures { get; set; }
    public int SignaturesRequired { get; set; }
    public string? RestrictionReason { get; set; }
    public decimal? UserTransactionLimit { get; set; }
    public decimal? UserDailyLimit { get; set; }
}

/// <summary>
/// Joint account summary information
/// </summary>
public class JointAccountSummary
{
    public Guid AccountId { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public bool IsJointAccount { get; set; }
    public bool RequiresMultipleSignatures { get; set; }
    public decimal? MultipleSignatureThreshold { get; set; }
    public int MinimumSignaturesRequired { get; set; }
    public int ActiveJointHoldersCount { get; set; }
    public List<JointAccountHolderDto> JointHolders { get; set; } = new();
}