using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Biller entity
/// </summary>
public class BillerRepository : Repository<Biller>, IBillerRepository
{
    public BillerRepository(BankDbContext context) : base(context)
    {
    }

    public async Task<List<Biller>> GetActiveBillersAsync()
    {
        return await _dbSet
            .Where(b => b.IsActive && !b.IsDeleted)
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<List<Biller>> GetBillersByCategoryAsync(BillerCategory category)
    {
        return await _dbSet
            .Where(b => b.Category == category && b.IsActive && !b.IsDeleted)
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<List<Biller>> SearchBillersByNameAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return new List<Biller>();

        return await _dbSet
            .Where(b => b.IsActive && !b.IsDeleted && 
                       b.Name.Contains(searchTerm))
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(string accountNumber, string routingNumber)
    {
        return await _dbSet
            .AnyAsync(b => b.AccountNumber == accountNumber && 
                          b.RoutingNumber == routingNumber && 
                          !b.IsDeleted);
    }

    public async Task<Biller?> GetBillerWithPaymentsAsync(Guid billerId)
    {
        return await _dbSet
            .Include(b => b.BillPayments)
            .FirstOrDefaultAsync(b => b.Id == billerId && !b.IsDeleted);
    }
}