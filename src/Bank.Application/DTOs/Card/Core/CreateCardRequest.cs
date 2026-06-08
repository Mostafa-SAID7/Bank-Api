using Bank.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bank.Application.DTOs.Card.Core;

/// <summary>
/// Request DTO for creating a new card
/// </summary>
public class CreateCardRequest
{
    /// <summary>
    /// Account ID to issue card for
    /// </summary>
    [Required]
    public Guid AccountId { get; set; }

    /// <summary>
    /// Type of card to create
    /// </summary>
    [Required]
    public CardType CardType { get; set; }

    /// <summary>
    /// Optional custom card name
    /// </summary>
    public string? CardName { get; set; }

    /// <summary>
    /// Daily transaction limit
    /// </summary>
    public decimal? DailyLimit { get; set; }

    /// <summary>
    /// Monthly transaction limit
    /// </summary>
    public decimal? MonthlyLimit { get; set; }

    /// <summary>
    /// Enable contactless payments
    /// </summary>
    public bool ContactlessEnabled { get; set; } = true;

    /// <summary>
    /// Enable online transactions
    /// </summary>
    public bool OnlineTransactionsEnabled { get; set; } = true;

    /// <summary>
    /// Enable international transactions
    /// </summary>
    public bool InternationalTransactionsEnabled { get; set; } = false;
}
