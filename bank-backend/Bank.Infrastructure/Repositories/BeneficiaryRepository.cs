using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Beneficiary entity
/// </summary>
public class BeneficiaryRepository : Repository<Beneficiary>, IBeneficiaryRepository
{
    public BeneficiaryRepository(BankDbContext context) : base(context)
    {
    }

    public async Task<List<Beneficiary>> GetCustomerBeneficiariesAsync(Guid customerId, bool activeOnly = true)
    {
        var query = _context.Set<Beneficiary>()
            .Where(b => b.CustomerId == customerId);

        if (activeOnly)
        {
            query = query.Where(b => b.IsActive);
        }

        return await query
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(Guid customerId, string accountNumber, string bankCode)
    {
        return await _context.Set<Beneficiary>()
            .AnyAsync(b => b.CustomerId == customerId && 
                          b.AccountNumber == accountNumber && 
                          b.BankCode == bankCode &&
                          b.IsActive);
    }

    public async Task<List<Beneficiary>> GetBeneficiariesByCategoryAsync(Guid customerId, BeneficiaryCategory category)
    {
        return await _context.Set<Beneficiary>()
            .Where(b => b.CustomerId == customerId && 
                       b.Category == category && 
                       b.IsActive)
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<List<Beneficiary>> GetPendingVerificationAsync()
    {
        return await _context.Set<Beneficiary>()
            .Where(b => b.Status == BeneficiaryStatus.Pending && b.IsActive)
            .OrderBy(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Beneficiary>> GetBeneficiariesWithActivityAsync(Guid customerId, DateTime fromDate, DateTime toDate)
    {
        return await _context.Set<Beneficiary>()
            .Where(b => b.CustomerId == customerId && 
                       b.IsActive &&
                       b.LastTransferDate.HasValue &&
                       b.LastTransferDate >= fromDate &&
                       b.LastTransferDate <= toDate)
            .OrderByDescending(b => b.LastTransferDate)
            .ToListAsync();
    }
}