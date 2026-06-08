using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for MaturityNotice entity
/// </summary>
public interface IMaturityNoticeRepository : IRepository<MaturityNotice>
{
    Task<IEnumerable<MaturityNotice>> GetNoticesByDepositAsync(Guid depositId);
    Task<IEnumerable<MaturityNotice>> GetPendingNoticesAsync();
    Task<IEnumerable<MaturityNotice>> GetNoticesByTypeAsync(MaturityNoticeType noticeType);
    Task<MaturityNotice?> GetByNoticeNumberAsync(string noticeNumber);
}
