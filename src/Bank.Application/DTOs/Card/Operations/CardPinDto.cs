using System.ComponentModel.DataAnnotations;

namespace Bank.Application.DTOs.Card.Operations;

/// <summary>
/// Request to change card PIN
/// </summary>
public class CardPinChangeRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public Guid CustomerId { get; set; }
    
    [Required]
    [StringLength(4, MinimumLength = 4)]
    public string CurrentPin { get; set; } = string.Empty;
    
    [Required]
    [StringLength(4, MinimumLength = 4)]
    public string NewPin { get; set; } = string.Empty;
}

/// <summary>
/// Request to reset card PIN
/// </summary>
public class CardPinResetRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public Guid CustomerId { get; set; }
    
    [Required]
    public string VerificationCode { get; set; } = string.Empty;
}

/// <summary>
/// Result of PIN change operation
/// </summary>
public class CardPinChangeResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime? PinChangeDate { get; set; }
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Result of PIN reset operation
/// </summary>
public class CardPinResetResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? NewPin { get; set; }
    public DateTime? PinResetDate { get; set; }
    public List<string> Errors { get; set; } = new();
}


