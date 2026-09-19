using Bank.CoreBanking.Application;
using Bank.CoreBanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bank.CoreBanking.Infrastructure.Data.Repositories;

public sealed class AccountRepository : IAccountRepository
{
    private readonly CoreBankingDbContext _context;

    public AccountRepository(CoreBankingDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Account account, CancellationToken cancellationToken = default)
    {
        await _context.Accounts.AddAsync(account, cancellationToken);
    }

    public async Task UpdateAsync(Account account, CancellationToken cancellationToken = default)
    {
        _context.Accounts.Update(account);
        await Task.CompletedTask;
    }

    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber, cancellationToken);
    }

    public async Task<IReadOnlyList<Account>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _context.Accounts
            .Where(a => a.CustomerId == customerId)
            .ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
