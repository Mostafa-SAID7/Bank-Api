using Bank.Application.Interfaces;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for DepositCertificate entity
/// </summary>
public class DepositCertificateRepository : Repository<DepositCertificate>, IDepositCertificateRepository
{
    public DepositCertificateRepository(BankDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DepositCertificate>> GetCertificatesByDepositAsync(Guid depositId)
    {
        return await _context.Set<DepositCertificate>()
            .Where(c => c.FixedDepositId == depositId)
            .OrderByDescending(c => c.IssueDate)
            .ToListAsync();
    }

    public async Task<DepositCertificate?> GetByCertificateNumberAsync(string certificateNumber)
    {
        return await _context.Set<DepositCertificate>()
            .Include(c => c.FixedDeposit)
            .FirstOrDefaultAsync(c => c.CertificateNumber == certificateNumber);
    }

    public async Task<IEnumerable<DepositCertificate>> GetPendingDeliveryAsync()
    {
        return await _context.Set<DepositCertificate>()
            .Where(c => c.Status == DepositCertificateStatus.Generated ||
                       c.Status == DepositCertificateStatus.Issued)
            .Include(c => c.FixedDeposit)
            .ThenInclude(d => d.Customer)
            .OrderBy(c => c.IssueDate)
            .ToListAsync();
    }
}

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