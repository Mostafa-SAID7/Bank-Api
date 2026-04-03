namespace Bank.Application.DTOs.Transaction.Analytics;

/// <summary>
/// Transaction trends over time
/// </summary>
public class TransactionTrendDto
{
    public DateTime Date { get; set; }
    public int TransactionCount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AverageAmount { get; set; }
    public int CreditCount { get; set; }
    public int DebitCount { get; set; }
    public decimal CreditAmount { get; set; }
    public decimal DebitAmount { get; set; }
}
