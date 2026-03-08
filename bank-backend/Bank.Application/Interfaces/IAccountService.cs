using Bank.Domain.Entities;

namespace Bank.Application.Interfaces;

public interface IAccountService
{
    Task<Account?> GetAccountByIdAsync(Guid id);
    Task<Account?> GetAccountAsync(Guid id); // Alias for consistency
    Task<Account?> GetAccountByNumberAsync(string accountNumber);
    Task<IEnumerable<Account>> GetUserAccountsAsync(Guid userId);
    Task<IEnumerable<Account>> GetAllAccountsAsync();
    Task<Account> CreateAccountAsync(Guid userId, string accountHolderName);
    Task<bool> UpdateBalanceAsync(Guid accountId, decimal amount);
    Task<bool> CanUserAccessAccountAsync(Guid accountId, Guid userId);
    Task<bool> UpdateAccountAsync(Account account);
}
