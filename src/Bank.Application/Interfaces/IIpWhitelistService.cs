using Bank.Application.DTOs;
using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for managing IP whitelist for administrative and secure access
/// </summary>
public interface IIpWhitelistService
{
    /// <summary>
    /// Adds an IP address to the whitelist
    /// </summary>
    Task<IpWhitelistResult> AddIpToWhitelistAsync(string ipAddress, IpWhitelistType type, string description, Guid createdByUserId, string? ipRange = null, DateTime? expiresAt = null);

    /// <summary>
    /// Removes an IP address from the whitelist
    /// </summary>
    Task<bool> RemoveIpFromWhitelistAsync(Guid whitelistId);

    /// <summary>
    /// Approves a pending IP whitelist entry
    /// </summary>
    Task<bool> ApproveIpWhitelistAsync(Guid whitelistId, Guid approvedByUserId, string? notes = null);

    /// <summary>
    /// Revokes an active IP whitelist entry
    /// </summary>
    Task<bool> RevokeIpWhitelistAsync(Guid whitelistId);

    /// <summary>
    /// Checks if an IP address is whitelisted for a specific type of access
    /// </summary>
    Task<bool> IsIpWhitelistedAsync(string ipAddress, IpWhitelistType type);

    /// <summary>
    /// Gets all whitelist entries for a specific type
    /// </summary>
    Task<List<IpWhitelist>> GetWhitelistEntriesAsync(IpWhitelistType? type = null, bool activeOnly = true);

    /// <summary>
    /// Gets pending whitelist entries requiring approval
    /// </summary>
    Task<List<IpWhitelist>> GetPendingApprovalsAsync();

    /// <summary>
    /// Extends the expiry date of a whitelist entry
    /// </summary>
    Task<bool> ExtendWhitelistEntryAsync(Guid whitelistId, DateTime newExpiryDate);

    /// <summary>
    /// Cleans up expired whitelist entries
    /// </summary>
    Task CleanupExpiredEntriesAsync();

    /// <summary>
    /// Validates IP address format and range
    /// </summary>
    bool ValidateIpAddress(string ipAddress, string? ipRange = null);

    /// <summary>
    /// Gets whitelist statistics for monitoring
    /// </summary>
    Task<IpWhitelistStatistics> GetWhitelistStatisticsAsync();
}