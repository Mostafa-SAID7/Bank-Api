using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Tracks the history of card status changes for audit purposes
/// </summary>
public class CardStatusHistory : BaseEntity
{
    /// <summary>
    /// Card whose status changed
    /// </summary>
    public Guid CardId { get; set; }
    public Card Card { get; set; } = null!;
    
    /// <summary>
    /// Previous status
    /// </summary>
    public CardStatus PreviousStatus { get; set; }
    
    /// <summary>
    /// New status
    /// </summary>
    public CardStatus NewStatus { get; set; }
    
    /// <summary>
    /// Reason for status change
    /// </summary>
    public string? Reason { get; set; }
    
    /// <summary>
    /// User who initiated the status change
    /// </summary>
    public Guid? ChangedBy { get; set; }
    public User? ChangedByUser { get; set; }
    
    /// <summary>
    /// Date and time of status change
    /// </summary>
    public DateTime ChangeDate { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Additional notes about the status change
    /// </summary>
    public string? Notes { get; set; }
    
    /// <summary>
    /// Channel through which status change was initiated
    /// </summary>
    public string? Channel { get; set; } // e.g., "Online", "Phone", "Branch", "System"
    
    /// <summary>
    /// IP address from which change was initiated (if applicable)
    /// </summary>
    public string? IpAddress { get; set; }
}