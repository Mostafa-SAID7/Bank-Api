using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

public interface IJointAccountService
{
    Task<bool> AddJointHolderAsync(int accountId, Guid userId, JointAccountRole role, int addedByUserId);
    Task<bool> RemoveJointHolderAsync(int accountId, Guid userId, int removedByUserId);
    Task<bool> UpdateJointHolderRoleAsync(int accountId, Guid userId, JointAccountRole newRole, int updatedByUserId);
    Task<List<JointAccountHolder>> GetJointHoldersAsync(int accountId);
    Task<bool> CanUserAccessAccountAsync(int accountId, Guid userId);
    Task<bool> CanUserPerformTransactionAsync(int accountId, Guid userId, decimal amount);
    Task<bool> RequiresMultipleSignaturesAsync(int accountId, decimal amount);
    Task<List<Account>> GetAccountsForUserAsync(Guid userId);
    Task<bool> ConvertToJointAccountAsync(int accountId, int convertedByUserId);
    Task<bool> ConvertToSingleAccountAsync(int accountId, Guid remainingHolderId, int convertedByUserId);
}