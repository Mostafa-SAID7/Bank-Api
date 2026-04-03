using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for Beneficiary entity
/// </summary>
public interface IBeneficiaryRepository : IRepository<Beneficiary>
{
    /// <summary>
    /// Get all active beneficiaries for a customer
    /// </summary>
    Task<List<Beneficiary>> GetCustomerBeneficiariesAsync(Guid customerId, bool activeOnly = true);
    
    /// <summary>
    /// Check if beneficiary exists for customer with same account details
    /// </summary>
    Task<bool> ExistsAsync(Guid customerId, string accountNumber, string bankCode);
    
    /// <summary>
    /// Get beneficiaries by category for a customer
    /// </summary>
    Task<List<Beneficiary>> GetBeneficiariesByCategoryAsync(Guid customerId, BeneficiaryCategory category);
    
    /// <summary>
    /// Get pending verification beneficiaries
    /// </summary>
    Task<List<Beneficiary>> GetPendingVerificationAsync();
    
    /// <summary>
    /// Get beneficiaries with transfer activity in date range
    /// </summary>
    Task<List<Beneficiary>> GetBeneficiariesWithActivityAsync(Guid customerId, DateTime fromDate, DateTime toDate);
}