using Bank.Application.DTOs;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service for managing deposit withdrawals and penalty calculations
/// </summary>
public interface IDepositWithdrawalService
{
    Task<DetailedWithdrawalCalculation> CalculateDetailedWithdrawalAsync(Guid depositId, decimal withdrawalAmount);
    Task<WithdrawalResult> ProcessEarlyWithdrawalWithDetailsAsync(Guid depositId, EarlyWithdrawalRequest request, Guid processedByUserId);
    Task<PenaltyFreePeriodsDto> GetPenaltyFreePeriodsAsync(Guid depositId);
    Task<IEnumerable<WithdrawalHistoryDto>> GetWithdrawalHistoryAsync(Guid depositId);
}
