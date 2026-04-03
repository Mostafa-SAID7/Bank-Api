using Bank.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bank.Application.DTOs.Card.Advanced;

/// <summary>
/// Request to update merchant restrictions
/// </summary>
public class CardMerchantRestrictionsRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public Guid CustomerId { get; set; }
    
    public List<MerchantCategory> BlockedCategories { get; set; } = new();
}

/// <summary>
/// Result of merchant restrictions update
/// </summary>
public class CardMerchantRestrictionsResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<MerchantCategory> BlockedCategories { get; set; } = new();
    public List<string> Errors { get; set; } = new();
}


