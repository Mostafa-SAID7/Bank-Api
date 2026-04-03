using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

public interface IJointAccountService
{
    Task<bool> AddJointHolderAsync(Guid accountId, Guid userId, JointAccountRole role, Guid addedByUserId);
    Task<bool> RemoveJointHolderAsync(Guid accountId, Guid userId, Guid removedByUserId);
    Task<bool> UpdateJointHolderRoleAsync(Guid accountId, Guid userId, JointAccountRole newRole, Guid updatedByUserId);
    Task<List<JointAccountHolder>> GetJointHoldersAsync(Guid accountId);
    Task<bool> CanUserAccessAccountAsync(Guid accountId, Guid userId);
    Task<bool> CanUserPerformTransactionAsync(Guid accountId, Guid userId, decimal amount);
    Task<bool> RequiresMultipleSignaturesAsync(Guid accountId, decimal amount);
    Task<List<Account>> GetAccountsForUserAsync(Guid userId);
    Task<bool> ConvertToJointAccountAsync(Guid accountId, Guid convertedByUserId);
    Task<bool> ConvertToSingleAccountAsync(Guid accountId, Guid remainingHolderId, Guid convertedByUserId);
}