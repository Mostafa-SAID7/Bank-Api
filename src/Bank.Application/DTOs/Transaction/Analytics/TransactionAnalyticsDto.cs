using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Transaction.Analytics;

/// <summary>
/// Transaction statistics and analytics
/// </summary>
public class TransactionStatistics
{
    public int TotalTransactions { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AverageAmount { get; set; }
    public int CreditTransactions { get; set; }
    public int DebitTransactions { get; set; }
    public decimal TotalCredits { get; set; }
    public decimal TotalDebits { get; set; }
    public Dictionary<TransactionType, int> TransactionsByType { get; set; } = new();
    public Dictionary<TransactionStatus, int> TransactionsByStatus { get; set; } = new();
}
