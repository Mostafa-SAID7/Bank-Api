using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for IpWhitelist entity
/// </summary>
public interface IIpWhitelistRepository : IRepository<IpWhitelist>
{
    /// <summary>
    /// Gets IP whitelist entry by IP address and type
    /// </summary>
    Task<IpWhitelist?> GetByIpAddressAndTypeAsync(string ipAddress, IpWhitelistType type);

    /// <summary>
    /// Gets all active entries for a specific type
    /// </summary>
    Task<List<IpWhitelist>> GetActiveEntriesByTypeAsync(IpWhitelistType type);

    /// <summary>
    /// Gets all entries for a specific type (active and inactive)
    /// </summary>
    Task<List<IpWhitelist>> GetEntriesByTypeAsync(IpWhitelistType type);

    /// <summary>
    /// Gets all active entries
    /// </summary>
    Task<List<IpWhitelist>> GetActiveEntriesAsync();

    /// <summary>
    /// Gets all entries (active and inactive)
    /// </summary>
    Task<List<IpWhitelist>> GetAllEntriesAsync();

    /// <summary>
    /// Gets entries pending approval
    /// </summary>
    Task<List<IpWhitelist>> GetPendingApprovalsAsync();

    /// <summary>
    /// Gets expired entries
    /// </summary>
    Task<List<IpWhitelist>> GetExpiredEntriesAsync();

    /// <summary>
    /// Gets entries created by a specific user
    /// </summary>
    Task<List<IpWhitelist>> GetEntriesByCreatorAsync(Guid createdByUserId);
}