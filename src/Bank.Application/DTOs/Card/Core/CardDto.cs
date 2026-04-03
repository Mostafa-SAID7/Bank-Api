using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Card.Core;

/// <summary>
/// Card data transfer object
/// </summary>
public class CardDto
{
    public string CardHolderName { get; set; }
    public string Type { get; set; }
    public Guid Id { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public CardType CardType { get; set; }
    public CardStatus Status { get; set; }
    public DateTime ExpiryDate { get; set; }
    public Guid AccountId { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}



