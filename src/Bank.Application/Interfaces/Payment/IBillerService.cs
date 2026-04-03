using Bank.Application.DTOs.Payment.Biller;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for biller management and queries
/// </summary>
public interface IBillerService
{
    /// <summary>
    /// Get all available active billers
    /// </summary>
    Task<List<BillerDto>> GetAvailableBillersAsync();

    /// <summary>
    /// Get billers by category
    /// </summary>
    Task<List<BillerDto>> GetBillersByCategoryAsync(BillerCategory category);

    /// <summary>
    /// Search billers by name or other criteria
    /// </summary>
    Task<List<BillerDto>> SearchBillersAsync(BillerSearchRequest request);

    /// <summary>
    /// Get biller details by ID
    /// </summary>
    Task<BillerDto?> GetBillerByIdAsync(Guid billerId);
}
