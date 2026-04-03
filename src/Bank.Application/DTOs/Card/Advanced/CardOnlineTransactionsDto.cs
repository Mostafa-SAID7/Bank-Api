using System.ComponentModel.DataAnnotations;

namespace Bank.Application.DTOs.Card.Advanced;

/// <summary>
/// Request to update online transactions settings
/// </summary>
public class CardOnlineTransactionsRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public Guid CustomerId { get; set; }
    
    [Required]
    public bool Enabled { get; set; }
}

/// <summary>
/// Result of online transactions settings update
/// </summary>
public class CardOnlineTransactionsResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool OnlineTransactionsEnabled { get; set; }
    public List<string> Errors { get; set; } = new();
}


