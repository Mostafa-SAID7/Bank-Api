using Bank.Application.DTOs;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service for managing deposit-related notifications and notices
/// </summary>
public interface IDepositNotificationService
{
    Task<MaturityNoticeDto> GenerateMaturityNoticeAsync(Guid depositId, MaturityNoticeType noticeType, Guid generatedByUserId);
    Task<bool> SendMaturityNoticesAsync();
    Task<bool> ProcessCustomerResponseAsync(Guid noticeId, MaturityAction customerChoice, string? instructions, Guid processedByUserId);
}
