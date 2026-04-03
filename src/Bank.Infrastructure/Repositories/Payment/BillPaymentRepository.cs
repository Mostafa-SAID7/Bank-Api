using Bank.Domain.Common;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories.Payment;

/// <summary>
/// Repository implementation for BillPayment entity
/// </summary>
public class BillPaymentRepository : Repository<BillPayment>, IBillPaymentRepository
{
    public BillPaymentRepository(BankDbContext context) : base(context)
    {
    }

    public async Task<PagedResult<BillPayment>> GetCustomerPaymentHistoryAsync(
        Guid customerId, 
        int pageNumber = 1, 
        int pageSize = 20,
        DateTime? fromDate = null,
        DateTime? toDate = null)
    {
        var query = _dbSet
            .Include(bp => bp.Biller)
            .Where(bp => bp.CustomerId == customerId && !bp.IsDeleted);

        if (fromDate.HasValue)
            query = query.Where(bp => bp.ScheduledDate >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(bp => bp.ScheduledDate <= toDate.Value);

        var totalCount = await query.CountAsync();
        
        var payments = await query
            .OrderByDescending(bp => bp.ScheduledDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new Bank.Domain.Common.PagedResult<BillPayment>
        {
            Items = payments,
            TotalCount = totalCount,
            Page = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<List<BillPayment>> GetScheduledPaymentsDueAsync(DateTime processingDate)
    {
        return await _dbSet
            .Include(bp => bp.Biller)
            .Include(bp => bp.Customer)
            .Where(bp => bp.Status == BillPaymentStatus.Pending && 
                        bp.ScheduledDate.Date <= processingDate.Date &&
                        !bp.IsDeleted)
            .OrderBy(bp => bp.ScheduledDate)
            .ToListAsync();
    }

    public async Task<List<BillPayment>> GetCustomerPendingPaymentsAsync(Guid customerId)
    {
        return await _dbSet
            .Include(bp => bp.Biller)
            .Where(bp => bp.CustomerId == customerId && 
                        bp.Status == BillPaymentStatus.Pending &&
                        !bp.IsDeleted)
            .OrderBy(bp => bp.ScheduledDate)
            .ToListAsync();
    }

    public async Task<List<BillPayment>> GetPaymentsByStatusAsync(BillPaymentStatus status)
    {
        return await _dbSet
            .Include(bp => bp.Biller)
            .Include(bp => bp.Customer)
            .Where(bp => bp.Status == status && !bp.IsDeleted)
            .OrderByDescending(bp => bp.ScheduledDate)
            .ToListAsync();
    }

    public async Task<BillPayment?> GetPaymentWithDetailsAsync(Guid paymentId)
    {
        return await _dbSet
            .Include(bp => bp.Biller)
            .Include(bp => bp.Customer)
            .Include(bp => bp.RecurringPayment)
            .FirstOrDefaultAsync(bp => bp.Id == paymentId && !bp.IsDeleted);
    }

    public async Task<List<BillPayment>> GetPaymentsByBillerAsync(Guid billerId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var query = _dbSet
            .Include(bp => bp.Customer)
            .Where(bp => bp.BillerId == billerId && !bp.IsDeleted);

        if (fromDate.HasValue)
            query = query.Where(bp => bp.ScheduledDate >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(bp => bp.ScheduledDate <= toDate.Value);

        return await query
            .OrderByDescending(bp => bp.ScheduledDate)
            .ToListAsync();
    }

    public async Task<List<BillPayment>> GetRecurringPaymentExecutionsAsync(Guid recurringPaymentId)
    {
        return await _dbSet
            .Include(bp => bp.Biller)
            .Where(bp => bp.RecurringPaymentId == recurringPaymentId && !bp.IsDeleted)
            .OrderByDescending(bp => bp.ScheduledDate)
            .ToListAsync();
    }

    public async Task<bool> HasPendingPaymentToBillerAsync(Guid customerId, Guid billerId)
    {
        return await _dbSet
            .AnyAsync(bp => bp.CustomerId == customerId && 
                           bp.BillerId == billerId && 
                           bp.Status == BillPaymentStatus.Pending &&
                           !bp.IsDeleted);
    }
}