using Bank.Domain.Enums;

namespace Bank.Application.DTOs;

public class IpWhitelistResult
{
    public bool Success { get; set; }
    public Guid? WhitelistId { get; set; }
    public string? ErrorMessage { get; set; }
    public bool RequiresApproval { get; set; }
}

public class IpWhitelistStatistics
{
    public int TotalActiveEntries { get; set; }
    public int PendingApprovals { get; set; }
    public int ExpiredEntries { get; set; }
    public Dictionary<IpWhitelistType, int> EntriesByType { get; set; } = new();
    public DateTime LastCleanupAt { get; set; }
}

public class IpWhitelistInfo
{
    public Guid Id { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string? IpRange { get; set; }
    public IpWhitelistType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string CreatedByUserName { get; set; } = string.Empty;
    public string? ApprovedByUserName { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}