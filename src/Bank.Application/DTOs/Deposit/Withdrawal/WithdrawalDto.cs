using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Deposit.Withdrawal;

/// <summary>
/// Withdrawal details data transfer object
/// </summary>
public class WithdrawalDetailsDto
{
    public Guid DepositId { get; set; }
    public decimal RequestedAmount { get; set; }
    public decimal AvailableBalance { get; set; }
    public decimal PenaltyAmount { get; set; }
    public decimal NetAmount { get; set; }
    public WithdrawalPenaltyType PenaltyType { get; set; }
    public string PenaltyDescription { get; set; } = string.Empty;
    public bool IsEarlyWithdrawal { get; set; }
    public int DaysBeforeMaturity { get; set; }
}

/// <summary>
/// Request for early withdrawal
/// </summary>
public class EarlyWithdrawalRequest
{
    public decimal WithdrawalAmount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public bool AcknowledgePenalty { get; set; }
}

/// <summary>
/// Request for partial withdrawal
/// </summary>
public class PartialWithdrawalRequest
{
    public decimal WithdrawalAmount { get; set; }
    public string Reason { get; set; } = string.Empty;
}

/// <summary>
/// Detailed withdrawal calculation
/// </summary>
public class DetailedWithdrawalCalculation
{
    public int DaysBeforeMaturity { get; set; }
    public decimal PenaltyAmount { get; set; }
    public string PenaltyDescription { get; set; }
    public string PenaltyType { get; set; }
    public Guid DepositId { get; set; }
    public string DepositNumber { get; set; } = string.Empty;
    public decimal RequestedAmount { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal AccruedInterest { get; set; }
    public decimal AvailableBalance { get; set; }
    public DateTime MaturityDate { get; set; }
    public int DaysToMaturity { get; set; }
    public bool IsEarlyWithdrawal { get; set; }
    
    public List<PenaltyDetail> PenaltyDetails { get; set; } = new();
    public decimal TotalPenalty { get; set; }
    public decimal NetAmount { get; set; }
    public decimal RemainingBalance { get; set; }
    
    public WithdrawalValidationResult ValidationResults { get; set; } = new();
}

/// <summary>
/// Penalty detail information
/// </summary>
public class PenaltyDetail
{
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// Withdrawal validation result
/// </summary>
public class WithdrawalValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Withdrawal operation result
/// </summary>
public class WithdrawalResult
{
    public Guid DepositId { get; set; }
    public bool Success { get; set; }
    public decimal NetAmountPaid { get; set; }
    public decimal PenaltyApplied { get; set; }
    public string TransactionReference { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Penalty-free periods information
/// </summary>
public class PenaltyFreePeriodsDto
{
    public Guid DepositId { get; set; }
    public bool HasPenaltyFreePeriods { get; set; }
    public int PenaltyFreeDays { get; set; }
    public DateTime? PenaltyFreeUntil { get; set; }
    public bool IsCurrentlyPenaltyFree { get; set; }
    public int? DaysUntilPenaltyFree { get; set; }
}

/// <summary>
/// Withdrawal history entry
/// </summary>
public class WithdrawalHistoryDto
{
    public Guid TransactionId { get; set; }
    public string TransactionReference { get; set; } = string.Empty;
    public DepositTransactionType WithdrawalType { get; set; }
    public decimal Amount { get; set; }
    public decimal PenaltyApplied { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public Guid? ProcessedBy { get; set; }
}



