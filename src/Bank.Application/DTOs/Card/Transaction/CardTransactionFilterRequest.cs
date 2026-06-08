namespace Bank.Application.DTOs.Card.Transaction;

/// <summary>
/// Query filter request for card transactions with pagination and sorting
/// </summary>
public class CardTransactionFilterRequest
{
    /// <summary>
    /// Start date for transaction filtering
    /// </summary>
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// End date for transaction filtering
    /// </summary>
    public DateTime? ToDate { get; set; }

    /// <summary>
    /// Transaction type filter (e.g., "Purchase", "Withdrawal")
    /// </summary>
    public string? TransactionType { get; set; }

    /// <summary>
    /// Transaction status filter (e.g., "Completed", "Pending")
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Minimum transaction amount filter
    /// </summary>
    public decimal? MinAmount { get; set; }

    /// <summary>
    /// Maximum transaction amount filter
    /// </summary>
    public decimal? MaxAmount { get; set; }

    /// <summary>
    /// Merchant name filter
    /// </summary>
    public string? MerchantName { get; set; }

    /// <summary>
    /// Merchant category filter
    /// </summary>
    public string? MerchantCategory { get; set; }

    /// <summary>
    /// Filter for international transactions
    /// </summary>
    public bool? IsInternational { get; set; }

    /// <summary>
    /// Page number for pagination (default: 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of items per page (default: 20, max: 100)
    /// </summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// Field name to sort by
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Whether to sort in descending order (default: true)
    /// </summary>
    public bool SortDescending { get; set; } = true;
}
