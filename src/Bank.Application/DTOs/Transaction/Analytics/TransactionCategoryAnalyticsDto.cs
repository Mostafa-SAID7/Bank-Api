namespace Bank.Application.DTOs.Transaction.Analytics;

/// <summary>
/// Transaction analytics by category
/// </summary>
public class TransactionCategoryAnalyticsDto
{
    public string Category { get; set; } = string.Empty;
    public int TransactionCount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AverageAmount { get; set; }
    public decimal PercentageOfTotal { get; set; }
    public DateTime? LastTransactionDate { get; set; }
}
