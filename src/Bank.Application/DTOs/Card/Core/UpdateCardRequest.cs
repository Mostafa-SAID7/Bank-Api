using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Card.Core;

/// <summary>
/// Request DTO for updating card information
/// </summary>
public class UpdateCardRequest
{
    /// <summary>
    /// Card ID to update
    /// </summary>
    public Guid CardId { get; set; }

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
    /// Enable/disable contactless payments
    /// </summary>
    public bool? ContactlessEnabled { get; set; }

    /// <summary>
    /// Enable/disable online transactions
    /// </summary>
    public bool? OnlineTransactionsEnabled { get; set; }

    /// <summary>
    /// Enable/disable international transactions
    /// </summary>
    public bool? InternationalTransactionsEnabled { get; set; }
}
