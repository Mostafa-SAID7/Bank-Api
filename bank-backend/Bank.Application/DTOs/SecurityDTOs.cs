using Bank.Domain.Enums;

namespace Bank.Application.DTOs;

#region IP Whitelist DTOs

public class AddIpWhitelistRequest
{
    public string IpAddress { get; set; } = string.Empty;
    public IpWhitelistType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? IpRange { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

public class ApproveIpWhitelistRequest
{
    public string? Notes { get; set; }
}

#endregion

#region Password Policy DTOs

public class ValidatePasswordRequest
{
    public string Password { get; set; } = string.Empty;
    public PasswordComplexityLevel? ComplexityLevel { get; set; }
}

public class GeneratePasswordRequest
{
    public PasswordComplexityLevel ComplexityLevel { get; set; } = PasswordComplexityLevel.Standard;
}

#endregion

#region Account Lockout DTOs

public class LockAccountRequest
{
    public AccountLockoutReason Reason { get; set; }
    public TimeSpan? LockoutDuration { get; set; }
    public string? Notes { get; set; }
}

#endregion