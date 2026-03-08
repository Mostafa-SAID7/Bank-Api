using Bank.Domain.Entities;
using Bank.Domain.Interfaces;
using Bank.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for PasswordHistory entity
/// </summary>
public class PasswordHistoryRepository : Repository<PasswordHistory>, IPasswordHistoryRepository
{
    public PasswordHistoryRepository(BankDbContext context) : base(context)
    {
    }

    public async Task<List<PasswordHistory>> GetPasswordHistoryAsync(Guid userId, int? limit = null)
    {
        var query = _context.PasswordHistories
            .Include(h => h.User)
            .Where(h => h.UserId == userId)
            .OrderByDescending(h => h.PasswordSetAt);

        if (limit.HasValue)
        {
            query = (IOrderedQueryable<PasswordHistory>)query.Take(limit.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<List<PasswordHistory>> GetRecentPasswordsAsync(Guid userId, int count)
    {
        return await _context.PasswordHistories
            .Where(h => h.UserId == userId)
            .OrderByDescending(h => h.PasswordSetAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<PasswordHistory?> GetCurrentPasswordAsync(Guid userId)
    {
        return await _context.PasswordHistories
            .Include(h => h.User)
            .FirstOrDefaultAsync(h => h.UserId == userId && h.IsCurrentPassword);
    }

    public async Task MarkCurrentPasswordAsOldAsync(Guid userId)
    {
        var currentPasswords = await _context.PasswordHistories
            .Where(h => h.UserId == userId && h.IsCurrentPassword)
            .ToListAsync();

        foreach (var password in currentPasswords)
        {
            password.MarkAsOldPassword();
        }

        await _context.SaveChangesAsync();
    }

    public async Task CleanupOldPasswordsAsync(Guid userId, int keepCount)
    {
        var passwordsToDelete = await _context.PasswordHistories
            .Where(h => h.UserId == userId && !h.IsCurrentPassword)
            .OrderByDescending(h => h.PasswordSetAt)
            .Skip(keepCount - 1) // Keep one less than the count since current password is separate
            .ToListAsync();

        if (passwordsToDelete.Any())
        {
            _context.PasswordHistories.RemoveRange(passwordsToDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<PasswordHistory?> GetByPasswordHashAsync(Guid userId, string passwordHash)
    {
        return await _context.PasswordHistories
            .FirstOrDefaultAsync(h => h.UserId == userId && h.PasswordHash == passwordHash);
    }
}