using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories.Statement;

/// <summary>
/// Repository implementation for AccountStatement entities
/// </summary>
public class StatementRepository : Repository<AccountStatement>, IStatementRepository
{
    public StatementRepository(BankDbContext context) : base(context)
    {
    }

    public async Task<List<AccountStatement>> GetByAccountIdAsync(Guid accountId, int? limit = null)
    {
        IQueryable<AccountStatement> query = _context.AccountStatements
            .Include(s => s.Account)
            .Include(s => s.Transactions)
            .Where(s => s.AccountId == accountId)
            .OrderByDescending(s => s.StatementDate);

        if (limit.HasValue)
            query = query.Take(limit.Value);

        return await query.ToListAsync();
    }

    public async Task<List<AccountStatement>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.AccountStatements
            .Include(s => s.Account)
            .Where(s => s.StatementDate >= startDate && s.StatementDate <= endDate)
            .OrderByDescending(s => s.StatementDate)
            .ToListAsync();
    }

    public async Task<List<AccountStatement>> GetByStatusAsync(StatementStatus status)
    {
        return await _context.AccountStatements
            .Include(s => s.Account)
            .Where(s => s.Status == status)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<int> GetNextStatementSequenceAsync(Guid accountId, DateTime statementDate)
    {
        var year = statementDate.Year;
        var month = statementDate.Month;
        
        var lastSequence = await _context.AccountStatements
            .Where(s => s.AccountId == accountId && 
                       s.StatementDate.Year == year && 
                       s.StatementDate.Month == month)
            .MaxAsync(s => (int?)s.StatementSequence) ?? 0;

        return lastSequence + 1;
    }

    public async Task<bool> ExistsForPeriodAsync(Guid accountId, DateTime startDate, DateTime endDate)
    {
        return await _context.AccountStatements
            .AnyAsync(s => s.AccountId == accountId &&
                          s.PeriodStartDate == startDate &&
                          s.PeriodEndDate == endDate &&
                          s.Status != StatementStatus.Cancelled);
    }

    public async Task<List<AccountStatement>> GetPendingDeliveryAsync()
    {
        return await _context.AccountStatements
            .Include(s => s.Account)
            .Where(s => s.Status == StatementStatus.Generated && 
                       !s.IsDelivered &&
                       s.DeliveryMethod != StatementDeliveryMethod.Download)
            .OrderBy(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<AccountStatement>> GetByDeliveryMethodAsync(StatementDeliveryMethod deliveryMethod)
    {
        return await _context.AccountStatements
            .Include(s => s.Account)
            .Where(s => s.DeliveryMethod == deliveryMethod)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }
}