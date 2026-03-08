using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Session entity
/// </summary>
public class SessionRepository : Repository<Session>, ISessionRepository
{
    public SessionRepository(BankDbContext context) : base(context)
    {
    }

    public async Task<Session?> GetBySessionTokenAsync(string sessionToken)
    {
        return await _context.Sessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.SessionToken == sessionToken);
    }

    public async Task<Session?> GetByRefreshTokenAsync(string refreshToken)
    {
        return await _context.Sessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.RefreshToken == refreshToken);
    }

    public async Task<List<Session>> GetActiveSessionsByUserIdAsync(Guid userId)
    {
        return await _context.Sessions
            .Include(s => s.User)
            .Where(s => s.UserId == userId && s.Status == SessionStatus.Active)
            .OrderByDescending(s => s.LastActivityAt)
            .ToListAsync();
    }

    public async Task<List<Session>> GetExpiredSessionsAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.Sessions
            .Where(s => s.Status == SessionStatus.Active && s.ExpiresAt < now)
            .ToListAsync();
    }

    public async Task<List<Session>> GetAllSessionsAsync()
    {
        return await _context.Sessions
            .Include(s => s.User)
            .ToListAsync();
    }

    public async Task<List<Session>> GetSessionsByIpAddressAsync(string ipAddress)
    {
        return await _context.Sessions
            .Include(s => s.User)
            .Where(s => s.IpAddress == ipAddress)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Session>> GetAdminSessionsAsync()
    {
        return await _context.Sessions
            .Include(s => s.User)
            .Where(s => s.IsAdminSession && s.Status == SessionStatus.Active)
            .OrderByDescending(s => s.LastActivityAt)
            .ToListAsync();
    }
}
