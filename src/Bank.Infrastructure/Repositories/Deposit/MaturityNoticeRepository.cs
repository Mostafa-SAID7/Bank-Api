using Bank.Application.Interfaces;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories.Deposit;

/// <summary>
/// Repository implementation for MaturityNotice entity
/// </summary>
public class MaturityNoticeRepository : Repository<MaturityNotice>, IMaturityNoticeRepository
{
    public MaturityNoticeRepository(BankDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MaturityNotice>> GetNoticesByDepositAsync(Guid depositId)
    {
        return await _context.Set<MaturityNotice>()
            .Where(n => n.FixedDepositId == depositId)
            .OrderByDescending(n => n.NoticeDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<MaturityNotice>> GetPendingNoticesAsync()
    {
        return await _context.Set<MaturityNotice>()
            .Where(n => n.Status == NotificationStatus.Pending ||
                       n.Status == NotificationStatus.Failed)
            .Include(n => n.FixedDeposit)
            .ThenInclude(d => d.Customer)
            .OrderBy(n => n.NoticeDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<MaturityNotice>> GetNoticesByTypeAsync(MaturityNoticeType noticeType)
    {
        return await _context.Set<MaturityNotice>()
            .Where(n => n.NoticeType == noticeType)
            .Include(n => n.FixedDeposit)
            .OrderByDescending(n => n.NoticeDate)
            .ToListAsync();
    }

    public async Task<MaturityNotice?> GetByNoticeNumberAsync(string noticeNumber)
    {
        return await _context.Set<MaturityNotice>()
            .Include(n => n.FixedDeposit)
            .ThenInclude(d => d.Customer)
            .FirstOrDefaultAsync(n => n.NoticeNumber == noticeNumber);
    }
}
