using Bank.Application.DTOs;
using Bank.Domain.Common;
using Bank.Domain.Entities;
using Bank.Domain.Interfaces;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for BillPresentment entity
/// </summary>
public class BillPresentmentRepository : Repository<BillPresentment>, IBillPresentmentRepository
{
    public BillPresentmentRepository(BankDbContext context) : base(context)
    {
    }

    public async Task<List<BillPresentment>> GetCustomerBillPresentmentsAsync(Guid customerId, BillPresentmentStatus? status = null)
    {
        var query = _dbSet
            .Include(bp => bp.Biller)
            .Where(bp => bp.CustomerId == customerId && !bp.IsDeleted);

        if (status.HasValue)
        {
            query = query.Where(bp => bp.Status == status.Value);
        }

        return await query
            .OrderByDescending(bp => bp.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<BillPresentment>> GetBillPresentmentsByBillerAsync(Guid billerId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var query = _dbSet
            .Include(bp => bp.Customer)
            .Where(bp => bp.BillerId == billerId && !bp.IsDeleted);

        if (fromDate.HasValue)
        {
            query = query.Where(bp => bp.CreatedAt >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(bp => bp.CreatedAt <= toDate.Value);
        }

        return await query
            .OrderByDescending(bp => bp.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<BillPresentment>> GetOverdueBillPresentmentsAsync()
    {
        var today = DateTime.UtcNow.Date;
        
        return await _dbSet
            .Include(bp => bp.Customer)
            .Include(bp => bp.Biller)
            .Where(bp => bp.Status == BillPresentmentStatus.Available && 
                        bp.DueDate.Date < today &&
                        !bp.IsDeleted)
            .OrderBy(bp => bp.DueDate)
            .ToListAsync();
    }

    public async Task<BillPresentment?> GetBillPresentmentWithDetailsAsync(Guid presentmentId)
    {
        return await _dbSet
            .Include(bp => bp.Customer)
            .Include(bp => bp.Biller)
            .Include(bp => bp.Payment)
            .FirstOrDefaultAsync(bp => bp.Id == presentmentId && !bp.IsDeleted);
    }

    public async Task<List<BillPresentment>> GetUnpaidBillPresentmentsAsync(Guid customerId, Guid billerId)
    {
        return await _dbSet
            .Include(bp => bp.Biller)
            .Where(bp => bp.CustomerId == customerId && 
                        bp.BillerId == billerId &&
                        bp.Status != BillPresentmentStatus.Paid &&
                        bp.Status != BillPresentmentStatus.Cancelled &&
                        !bp.IsDeleted)
            .OrderBy(bp => bp.DueDate)
            .ToListAsync();
    }

    public async Task<List<BillPresentment>> GetBillPresentmentsDueWithinDaysAsync(int days)
    {
        var targetDate = DateTime.UtcNow.Date.AddDays(days);
        
        return await _dbSet
            .Include(bp => bp.Customer)
            .Include(bp => bp.Biller)
            .Where(bp => bp.Status == BillPresentmentStatus.Available &&
                        bp.DueDate.Date <= targetDate &&
                        bp.DueDate.Date >= DateTime.UtcNow.Date &&
                        !bp.IsDeleted)
            .OrderBy(bp => bp.DueDate)
            .ToListAsync();
    }

    public async Task<bool> ExistsByExternalBillIdAsync(string externalBillId)
    {
        return await _dbSet
            .AnyAsync(bp => bp.ExternalBillId == externalBillId && !bp.IsDeleted);
    }

    public async Task<BillPresentment?> GetByExternalBillIdAsync(string externalBillId)
    {
        return await _dbSet
            .Include(bp => bp.Customer)
            .Include(bp => bp.Biller)
            .FirstOrDefaultAsync(bp => bp.ExternalBillId == externalBillId && !bp.IsDeleted);
    }
}