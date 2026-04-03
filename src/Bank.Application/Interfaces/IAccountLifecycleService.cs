using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

public interface IAccountLifecycleService
{
    Task<bool> CloseAccountAsync(Guid accountId, string reason, Guid userId);
    Task<bool> ReopenAccountAsync(Guid accountId, Guid userId);
    Task<bool> MarkAccountDormantAsync(Guid accountId);
    Task<bool> ReactivateAccountAsync(Guid accountId, Guid userId);
    Task<bool> SuspendAccountAsync(Guid accountId, string reason, Guid userId);
    Task<bool> UnsuspendAccountAsync(Guid accountId, Guid userId);
    Task<bool> FreezeAccountAsync(Guid accountId, string reason, Guid userId);
    Task<bool> UnfreezeAccountAsync(Guid accountId, Guid userId);
    Task<bool> ApplyHoldAsync(Guid accountId, decimal amount, string reason, Guid userId);
    Task<bool> ReleaseHoldAsync(Guid holdId, Guid userId);
    Task<bool> AddRestrictionAsync(Guid accountId, AccountRestrictionType restrictionType, string reason, Guid userId);
    Task<bool> RemoveRestrictionAsync(Guid restrictionId, Guid userId);
    Task<decimal> CalculateAccountFeesAsync(Guid accountId, DateTime fromDate, DateTime toDate);
    Task<bool> ApplyAccountFeesAsync(Guid accountId, Guid userId);
    Task<List<Account>> GetDormantAccountsAsync(int daysSinceLastActivity);
    Task<List<Account>> GetAccountsForFeeProcessingAsync();
    Task<bool> ProcessMonthlyMaintenanceFeesAsync();
}