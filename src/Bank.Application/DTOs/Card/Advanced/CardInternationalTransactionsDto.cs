using System.ComponentModel.DataAnnotations;

namespace Bank.Application.DTOs.Card.Advanced;

/// <summary>
/// Request to update international transactions settings
/// </summary>
public class CardInternationalTransactionsRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public Guid CustomerId { get; set; }
    
    [Required]
    public bool Enabled { get; set; }
}

/// <summary>
/// Result of international transactions settings update
/// </summary>
public class CardInternationalTransactionsResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool InternationalTransactionsEnabled { get; set; }
    public List<string> Errors { get; set; } = new();
}


