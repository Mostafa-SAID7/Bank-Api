using Bank.Application.DTOs;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service for managing API rate limiting
/// </summary>
public interface IRateLimitingService
{
    /// <summary>
    /// Check if request is allowed based on rate limits
    /// </summary>
    Task<RateLimitResult> IsRequestAllowedAsync(string key, RateLimitPolicy policy);
    
    /// <summary>
    /// Check rate limit for user-based requests
    /// </summary>
    Task<RateLimitResult> IsUserRequestAllowedAsync(Guid userId, string action, RateLimitPolicy? customPolicy = null);
    
    /// <summary>
    /// Check rate limit for IP-based requests
    /// </summary>
    Task<RateLimitResult> IsIpRequestAllowedAsync(string ipAddress, string action, RateLimitPolicy? customPolicy = null);
    
    /// <summary>
    /// Reset rate limit for a specific key
    /// </summary>
    Task ResetRateLimitAsync(string key);
    
    /// <summary>
    /// Get current rate limit status
    /// </summary>
    Task<RateLimitStatus> GetRateLimitStatusAsync(string key);
}