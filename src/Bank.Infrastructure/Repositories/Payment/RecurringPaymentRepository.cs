using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Infrastructure.Data;
using Bank.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories.Payment;

public class RecurringPaymentRepository : IRecurringPaymentRepository
{
    private readonly BankDbContext _context;

    public RecurringPaymentRepository(BankDbContext context)
    {
        _context = context;
    }

    public async Task<RecurringPayment?> GetByIdAsync(Guid id)
    {
        return await _context.RecurringPayments
            .Include(rp => rp.FromAccount)
            .Include(rp => rp.ToAccount)
            .Include(rp => rp.CreatedByUser)
            .Include(rp => rp.Executions)
            .FirstOrDefaultAsync(rp => rp.Id == id);
    }

    public async Task<IEnumerable<RecurringPayment>> GetByUserIdAsync(Guid userId)
    {
        return await _context.RecurringPayments
            .Include(rp => rp.FromAccount)
            .Include(rp => rp.ToAccount)
            .Where(rp => rp.CreatedByUserId == userId)
            .OrderByDescending(rp => rp.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<RecurringPayment>> GetByAccountIdAsync(Guid accountId)
    {
        return await _context.RecurringPayments
            .Include(rp => rp.FromAccount)
            .Include(rp => rp.ToAccount)
            .Where(rp => rp.FromAccountId == accountId || rp.ToAccountId == accountId)
            .OrderByDescending(rp => rp.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<RecurringPayment>> GetDuePaymentsAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.RecurringPayments
            .Include(rp => rp.FromAccount)
            .Include(rp => rp.ToAccount)
            .Where(rp => rp.Status == RecurringPaymentStatus.Active &&
                        rp.NextExecutionDate <= now &&
                        (rp.EndDate == null || rp.EndDate > now) &&
                        (rp.MaxOccurrences == null || rp.ExecutionCount < rp.MaxOccurrences))
            .ToListAsync();
    }

    public async Task<RecurringPayment> AddAsync(RecurringPayment recurringPayment)
    {
        _context.RecurringPayments.Add(recurringPayment);
        await _context.SaveChangesAsync();
        return recurringPayment;
    }

    public async Task UpdateAsync(RecurringPayment recurringPayment)
    {
        _context.RecurringPayments.Update(recurringPayment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var recurringPayment = await _context.RecurringPayments.FindAsync(id);
        if (recurringPayment != null)
        {
            _context.RecurringPayments.Remove(recurringPayment);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<RecurringPaymentExecution> AddExecutionAsync(RecurringPaymentExecution execution)
    {
        _context.RecurringPaymentExecutions.Add(execution);
        await _context.SaveChangesAsync();
        return execution;
    }

    public async Task<IEnumerable<RecurringPaymentExecution>> GetExecutionHistoryAsync(Guid recurringPaymentId)
    {
        return await _context.RecurringPaymentExecutions
            .Include(rpe => rpe.Transaction)
            .Where(rpe => rpe.RecurringPaymentId == recurringPaymentId)
            .OrderByDescending(rpe => rpe.ScheduledDate)
            .ToListAsync();
    }
}
