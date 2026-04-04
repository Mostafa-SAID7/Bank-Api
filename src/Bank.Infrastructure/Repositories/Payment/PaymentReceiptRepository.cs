using Bank.Domain.Common;
using Bank.Domain.Entities;
using Bank.Domain.Interfaces;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for PaymentReceipt entity
/// </summary>
public class PaymentReceiptRepository : Repository<PaymentReceipt>, IPaymentReceiptRepository
{
    public PaymentReceiptRepository(BankDbContext context) : base(context)
    {
    }

    public async Task<PaymentReceipt?> GetByPaymentIdAsync(Guid paymentId)
    {
        return await _dbSet
            .Include(pr => pr.Payment)
            .Include(pr => pr.Customer)
            .FirstOrDefaultAsync(pr => pr.PaymentId == paymentId && !pr.IsDeleted);
    }

    public async Task<PagedResult<PaymentReceipt>> GetCustomerReceiptsAsync(
        Guid customerId, 
        int pageNumber = 1, 
        int pageSize = 20,
        DateTime? fromDate = null,
        DateTime? toDate = null)
    {
        var query = _dbSet
            .Include(pr => pr.Payment)
            .Where(pr => pr.CustomerId == customerId && !pr.IsDeleted);

        if (fromDate.HasValue)
        {
            query = query.Where(pr => pr.CreatedAt >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(pr => pr.CreatedAt <= toDate.Value);
        }

        var totalCount = await query.CountAsync();
        
        var receipts = await query
            .OrderByDescending(pr => pr.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<PaymentReceipt>
        {
            Items = receipts,
            TotalCount = totalCount,
            Page = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<PaymentReceipt?> GetByReceiptNumberAsync(string receiptNumber)
    {
        return await _dbSet
            .Include(pr => pr.Payment)
            .Include(pr => pr.Customer)
            .FirstOrDefaultAsync(pr => pr.ReceiptNumber == receiptNumber && !pr.IsDeleted);
    }

    public async Task<List<PaymentReceipt>> GetByConfirmationNumberAsync(string confirmationNumber)
    {
        return await _dbSet
            .Include(pr => pr.Payment)
            .Include(pr => pr.Customer)
            .Where(pr => pr.ConfirmationNumber == confirmationNumber && !pr.IsDeleted)
            .ToListAsync();
    }

    public async Task<bool> ReceiptNumberExistsAsync(string receiptNumber)
    {
        return await _dbSet
            .AnyAsync(pr => pr.ReceiptNumber == receiptNumber && !pr.IsDeleted);
    }
}
