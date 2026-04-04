using Bank.Domain.Entities;
using Account = Bank.Domain.Entities.Account;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for AccountLockout entity
/// </summary>
public class AccountLockoutRepository : Repository<AccountLockout>, IAccountLockoutRepository
{
    public AccountLockoutRepository(BankDbContext context) : base(context)
    {
    }

    public async Task<AccountLockout?> GetByUserIdAsync(Guid userId)
    {
        return await _context.AccountLockouts
            .Include(l => l.User)
            .Include(l => l.LockedByUser)
            .FirstOrDefaultAsync(l => l.UserId == userId);
    }

    public async Task<List<AccountLockout>> GetCurrentlyLockedAccountsAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.AccountLockouts
            .Include(l => l.User)
            .Include(l => l.LockedByUser)
            .Where(l => l.IsCurrentlyLocked && 
                       (l.LockedUntil == null || l.LockedUntil > now))
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<AccountLockout>> GetExpiredLockoutsAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.AccountLockouts
            .Where(l => l.IsCurrentlyLocked && 
                       l.LockedUntil.HasValue && 
                       l.LockedUntil < now)
            .ToListAsync();
    }

    public async Task<List<AccountLockout>> GetLockoutHistoryAsync(Guid userId)
    {
        return await _context.AccountLockouts
            .Include(l => l.User)
            .Include(l => l.LockedByUser)
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<AccountLockout>> GetAllLockoutsAsync()
    {
        return await _context.AccountLockouts
            .Include(l => l.User)
            .Include(l => l.LockedByUser)
            .ToListAsync();
    }

    public async Task<List<AccountLockout>> GetLockoutsByReasonAsync(AccountLockoutReason reason)
    {
        return await _context.AccountLockouts
            .Include(l => l.User)
            .Include(l => l.LockedByUser)
            .Where(l => l.LockoutReason == reason)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }
}


