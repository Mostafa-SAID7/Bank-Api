using Bank.Application.DTOs;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for checking transfer eligibility and limits
/// </summary>
public interface ITransferEligibilityService
{
    /// <summary>
    /// Check if a beneficiary is eligible to receive transfers
    /// </summary>
    Task<TransferEligibilityResult> CheckTransferEligibilityAsync(TransferEligibilityRequest request);
    
    /// <summary>
    /// Get remaining transfer limits for a beneficiary
    /// </summary>
    Task<BeneficiaryLimitsResult> GetRemainingLimitsAsync(Guid beneficiaryId, DateTime? forDate = null);
    
    /// <summary>
    /// Check daily transfer limits for a beneficiary
    /// </summary>
    Task<LimitCheckResult> CheckDailyLimitsAsync(Guid beneficiaryId, decimal amount, DateTime? transferDate = null);
    
    /// <summary>
    /// Check monthly transfer limits for a beneficiary
    /// </summary>
    Task<LimitCheckResult> CheckMonthlyLimitsAsync(Guid beneficiaryId, decimal amount, DateTime? transferDate = null);
    
    /// <summary>
    /// Get transfer history summary for limit calculations
    /// </summary>
    Task<TransferHistorySummary> GetTransferHistorySummaryAsync(Guid beneficiaryId, DateTime fromDate, DateTime toDate);
    
    /// <summary>
    /// Pre-validate transfer before execution
    /// </summary>
    Task<TransferPreValidationResult> PreValidateTransferAsync(TransferPreValidationRequest request);
}