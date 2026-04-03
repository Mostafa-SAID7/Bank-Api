using Bank.Domain.Common;
using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for Biller entity operations
/// </summary>
public interface IBillerRepository : IRepository<Biller>
{
    /// <summary>
    /// Get all active billers
    /// </summary>
    Task<List<Biller>> GetActiveBillersAsync();

    /// <summary>
    /// Get billers by category
    /// </summary>
    Task<List<Biller>> GetBillersByCategoryAsync(BillerCategory category);

    /// <summary>
    /// Search billers by name
    /// </summary>
    Task<List<Biller>> SearchBillersByNameAsync(string searchTerm);

    /// <summary>
    /// Check if a biller exists by account number and routing number
    /// </summary>
    Task<bool> ExistsAsync(string accountNumber, string routingNumber);

    /// <summary>
    /// Get biller with payment history
    /// </summary>
    Task<Biller?> GetBillerWithPaymentsAsync(Guid billerId);
}