using System.ComponentModel.DataAnnotations;

namespace Bank.Application.DTOs.Card.Advanced;

/// <summary>
/// Request to update contactless settings
/// </summary>
public class CardContactlessRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public Guid CustomerId { get; set; }
    
    [Required]
    public bool Enabled { get; set; }
}

/// <summary>
/// Result of contactless settings update
/// </summary>
public class CardContactlessResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool ContactlessEnabled { get; set; }
    public List<string> Errors { get; set; } = new();
}


