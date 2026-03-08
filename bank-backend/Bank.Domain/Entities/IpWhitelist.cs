using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// IP whitelist entity for controlling administrative and high-security access
/// </summary>
public class IpWhitelist : BaseEntity
{
    public string IpAddress { get; private set; } = string.Empty;
    public string? IpRange { get; private set; } // CIDR notation for IP ranges
    public IpWhitelistType Type { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public DateTime? ExpiresAt { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public Guid? ApprovedByUserId { get; private set; }
    public DateTime? ApprovedAt { get; private set; }
    public string? ApprovalNotes { get; private set; }

    // Navigation properties
    public User CreatedByUser { get; private set; } = null!;
    public User? ApprovedByUser { get; private set; }

    // Private constructor for EF Core
    private IpWhitelist() { }

    public IpWhitelist(
        string ipAddress,
        IpWhitelistType type,
        string description,
        Guid createdByUserId,
        string? ipRange = null,
        DateTime? expiresAt = null)
    {
        IpAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
        IpRange = ipRange;
        Type = type;
        Description = description ?? throw new ArgumentNullException(nameof(description));
        CreatedByUserId = createdByUserId;
        ExpiresAt = expiresAt;
        IsActive = false; // Requires approval by default
        CreatedAt = DateTime.UtcNow;
    }

    public void Approve(Guid approvedByUserId, string? notes = null)
    {
        IsActive = true;
        ApprovedByUserId = approvedByUserId;
        ApprovedAt = DateTime.UtcNow;
        ApprovalNotes = notes;
    }

    public void Revoke()
    {
        IsActive = false;
    }

    public void Extend(DateTime newExpiryDate)
    {
        if (newExpiryDate > DateTime.UtcNow)
        {
            ExpiresAt = newExpiryDate;
        }
    }

    public bool IsExpired()
    {
        return ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value;
    }

    public bool IsValidForAccess()
    {
        return IsActive && !IsExpired();
    }

    public bool MatchesIpAddress(string clientIpAddress)
    {
        if (string.IsNullOrEmpty(clientIpAddress))
            return false;

        // Exact match
        if (IpAddress.Equals(clientIpAddress, StringComparison.OrdinalIgnoreCase))
            return true;

        // Range match (if CIDR notation is provided)
        if (!string.IsNullOrEmpty(IpRange))
        {
            return IsIpInRange(clientIpAddress, IpRange);
        }

        return false;
    }

    private static bool IsIpInRange(string ipAddress, string cidrRange)
    {
        try
        {
            var parts = cidrRange.Split('/');
            if (parts.Length != 2)
                return false;

            var networkAddress = System.Net.IPAddress.Parse(parts[0]);
            var prefixLength = int.Parse(parts[1]);
            var clientIp = System.Net.IPAddress.Parse(ipAddress);

            // Convert to bytes for comparison
            var networkBytes = networkAddress.GetAddressBytes();
            var clientBytes = clientIp.GetAddressBytes();

            if (networkBytes.Length != clientBytes.Length)
                return false;

            // Calculate subnet mask
            var maskBytes = new byte[networkBytes.Length];
            var bitsToMask = prefixLength;
            
            for (int i = 0; i < maskBytes.Length; i++)
            {
                if (bitsToMask >= 8)
                {
                    maskBytes[i] = 0xFF;
                    bitsToMask -= 8;
                }
                else if (bitsToMask > 0)
                {
                    maskBytes[i] = (byte)(0xFF << (8 - bitsToMask));
                    bitsToMask = 0;
                }
                else
                {
                    maskBytes[i] = 0x00;
                }
            }

            // Apply mask and compare
            for (int i = 0; i < networkBytes.Length; i++)
            {
                if ((networkBytes[i] & maskBytes[i]) != (clientBytes[i] & maskBytes[i]))
                    return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}