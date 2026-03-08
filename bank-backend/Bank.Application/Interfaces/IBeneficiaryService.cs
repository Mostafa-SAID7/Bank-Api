using Bank.Application.DTOs;
using Bank.Domain.Entities;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for managing beneficiaries (payees) for fund transfers
/// </summary>
public interface IBeneficiaryService
{
    /// <summary>
    /// Add a new beneficiary for a customer
    /// </summary>
    Task<BeneficiaryResult> AddBeneficiaryAsync(Guid customerId, AddBeneficiaryRequest request);
    
    /// <summary>
    /// Update beneficiary information
    /// </summary>
    Task<BeneficiaryResult> UpdateBeneficiaryAsync(Guid beneficiaryId, UpdateBeneficiaryRequest request, Guid updatedByUserId);
    
    /// <summary>
    /// Get beneficiary by ID
    /// </summary>
    Task<BeneficiaryDto?> GetBeneficiaryByIdAsync(Guid beneficiaryId);
    
    /// <summary>
    /// Get all beneficiaries for a customer
    /// </summary>
    Task<List<BeneficiaryDto>> GetCustomerBeneficiariesAsync(Guid customerId, bool activeOnly = true);
    
    /// <summary>
    /// Search beneficiaries with criteria
    /// </summary>
    Task<BeneficiarySearchResult> SearchBeneficiariesAsync(BeneficiarySearchCriteria criteria);
    
    /// <summary>
    /// Verify a beneficiary account
    /// </summary>
    Task<BeneficiaryVerificationResult> VerifyBeneficiaryAsync(Guid beneficiaryId, Guid verifiedByUserId);
    
    /// <summary>
    /// Archive a beneficiary (soft delete while preserving history)
    /// </summary>
    Task<bool> ArchiveBeneficiaryAsync(Guid beneficiaryId, string reason, Guid archivedByUserId);
    
    /// <summary>
    /// Reactivate an archived beneficiary
    /// </summary>
    Task<bool> ReactivateBeneficiaryAsync(Guid beneficiaryId, Guid reactivatedByUserId);
    
    /// <summary>
    /// Check if beneficiary can receive transfers
    /// </summary>
    Task<bool> CanReceiveTransfersAsync(Guid beneficiaryId);
    
    /// <summary>
    /// Validate transfer amount against beneficiary limits
    /// </summary>
    Task<bool> ValidateTransferLimitsAsync(Guid beneficiaryId, decimal amount);
    
    /// <summary>
    /// Record a successful transfer to beneficiary
    /// </summary>
    Task RecordTransferAsync(Guid beneficiaryId, decimal amount);
    
    /// <summary>
    /// Get transfer history for a beneficiary
    /// </summary>
    Task<BeneficiaryTransferHistory> GetTransferHistoryAsync(Guid beneficiaryId, DateTime? fromDate = null, DateTime? toDate = null);
    
    /// <summary>
    /// Get beneficiary statistics for a customer
    /// </summary>
    Task<BeneficiaryStatistics> GetBeneficiaryStatisticsAsync(Guid customerId);
    
    /// <summary>
    /// Validate beneficiary account details
    /// </summary>
    Task<BeneficiaryVerificationResult> ValidateAccountDetailsAsync(AddBeneficiaryRequest request);
    
    /// <summary>
    /// Update beneficiary transfer limits
    /// </summary>
    Task<bool> UpdateTransferLimitsAsync(Guid beneficiaryId, decimal? dailyLimit, decimal? monthlyLimit, decimal? singleLimit, Guid updatedByUserId);
}