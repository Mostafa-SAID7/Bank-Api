using Bank.Domain.Entities;

namespace Bank.Application.Interfaces;

public interface IAccountService
{
    Task<Account?> GetAccountByIdAsync(Guid id);
    Task<Account?> GetAccountByNumberAsync(string accountNumber);
    Task<IEnumerable<Account>> GetUserAccountsAsync(Guid userId);
    Task<IEnumerable<Account>> GetAllAccountsAsync();
    Task<Account> CreateAccountAsync(Guid userId, string accountHolderName);
    Task<bool> UpdateBalanceAsync(Guid accountId, decimal amount);
}
