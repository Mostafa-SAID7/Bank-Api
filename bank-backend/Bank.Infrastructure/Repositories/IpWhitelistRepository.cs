using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Bank.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for IpWhitelist entity
/// </summary>
public class IpWhitelistRepository : Repository<IpWhitelist>, IIpWhitelistRepository
{
    public IpWhitelistRepository(BankDbContext context) : base(context)
    {
    }

    public async Task<IpWhitelist?> GetByIpAddressAndTypeAsync(string ipAddress, IpWhitelistType type)
    {
        return await _context.IpWhitelists
            .Include(w => w.CreatedByUser)
            .Include(w => w.ApprovedByUser)
            .FirstOrDefaultAsync(w => w.IpAddress == ipAddress && w.Type == type);
    }

    public async Task<List<IpWhitelist>> GetActiveEntriesByTypeAsync(IpWhitelistType type)
    {
        var now = DateTime.UtcNow;
        return await _context.IpWhitelists
            .Include(w => w.CreatedByUser)
            .Include(w => w.ApprovedByUser)
            .Where(w => w.Type == type && 
                       w.IsActive && 
                       (w.ExpiresAt == null || w.ExpiresAt > now))
            .OrderBy(w => w.IpAddress)
            .ToListAsync();
    }

    public async Task<List<IpWhitelist>> GetEntriesByTypeAsync(IpWhitelistType type)
    {
        return await _context.IpWhitelists
            .Include(w => w.CreatedByUser)
            .Include(w => w.ApprovedByUser)
            .Where(w => w.Type == type)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<IpWhitelist>> GetActiveEntriesAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.IpWhitelists
            .Include(w => w.CreatedByUser)
            .Include(w => w.ApprovedByUser)
            .Where(w => w.IsActive && 
                       (w.ExpiresAt == null || w.ExpiresAt > now))
            .OrderBy(w => w.Type)
            .ThenBy(w => w.IpAddress)
            .ToListAsync();
    }

    public async Task<List<IpWhitelist>> GetAllEntriesAsync()
    {
        return await _context.IpWhitelists
            .Include(w => w.CreatedByUser)
            .Include(w => w.ApprovedByUser)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<IpWhitelist>> GetPendingApprovalsAsync()
    {
        return await _context.IpWhitelists
            .Include(w => w.CreatedByUser)
            .Where(w => !w.IsActive && w.ApprovedByUserId == null)
            .OrderBy(w => w.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<IpWhitelist>> GetExpiredEntriesAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.IpWhitelists
            .Where(w => w.IsActive && 
                       w.ExpiresAt.HasValue && 
                       w.ExpiresAt < now)
            .ToListAsync();
    }

    public async Task<List<IpWhitelist>> GetEntriesByCreatorAsync(Guid createdByUserId)
    {
        return await _context.IpWhitelists
            .Include(w => w.CreatedByUser)
            .Include(w => w.ApprovedByUser)
            .Where(w => w.CreatedByUserId == createdByUserId)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();
    }
}