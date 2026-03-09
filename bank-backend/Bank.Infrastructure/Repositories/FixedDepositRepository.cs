using Bank.Application.Interfaces;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for FixedDeposit entity
/// </summary>
public class FixedDepositRepository : Repository<FixedDeposit>, IFixedDepositRepository
{
    public FixedDepositRepository(BankDbContext context) : base(context)
    {
    }

    public async Task<FixedDeposit?> GetByDepositNumberAsync(string depositNumber)
    {
        return await _context.Set<FixedDeposit>()
            .Include(d => d.Customer)
            .Include(d => d.DepositProduct)
            .Include(d => d.LinkedAccount)
            .FirstOrDefaultAsync(d => d.DepositNumber == depositNumber);
    }

    public async Task<IEnumerable<FixedDeposit>> GetCustomerDepositsAsync(Guid customerId)
    {
        return await _context.Set<FixedDeposit>()
            .Where(d => d.CustomerId == customerId)
            .Include(d => d.Customer)
            .Include(d => d.DepositProduct)
            .Include(d => d.LinkedAccount)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<FixedDeposit>> GetMaturingDepositsAsync(DateTime fromDate, DateTime toDate)
    {
        return await _context.Set<FixedDeposit>()
            .Where(d => d.Status == FixedDepositStatus.Active &&
                       d.MaturityDate >= fromDate &&
                       d.MaturityDate <= toDate)
            .Include(d => d.Customer)
            .Include(d => d.DepositProduct)
            .Include(d => d.LinkedAccount)
            .OrderBy(d => d.MaturityDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<FixedDeposit>> GetActiveDepositsAsync()
    {
        return await _context.Set<FixedDeposit>()
            .Where(d => d.Status == FixedDepositStatus.Active)
            .Include(d => d.Customer)
            .Include(d => d.DepositProduct)
            .Include(d => d.LinkedAccount)
            .ToListAsync();
    }

    public async Task<IEnumerable<FixedDeposit>> GetDepositsForInterestProcessingAsync()
    {
        return await _context.Set<FixedDeposit>()
            .Where(d => d.Status == FixedDepositStatus.Active &&
                       d.LastInterestCalculationDate < DateTime.UtcNow.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<FixedDeposit>> GetDepositsForAutoRenewalAsync()
    {
        return await _context.Set<FixedDeposit>()
            .Where(d => d.Status == FixedDepositStatus.Active &&
                       d.MaturityDate <= DateTime.UtcNow &&
                       d.AutoRenewalEnabled &&
                       d.CustomerConsentReceived)
            .Include(d => d.DepositProduct)
            .ToListAsync();
    }

    public async Task<FixedDeposit?> GetDepositWithDetailsAsync(Guid depositId)
    {
        return await _context.Set<FixedDeposit>()
            .Include(d => d.Customer)
            .Include(d => d.DepositProduct)
            .Include(d => d.LinkedAccount)
            .Include(d => d.Transactions)
            .Include(d => d.Certificates)
            .Include(d => d.MaturityNotices)
            .FirstOrDefaultAsync(d => d.Id == depositId);
    }
}