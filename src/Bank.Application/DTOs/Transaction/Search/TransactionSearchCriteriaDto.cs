using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Transaction.Search;

/// <summary>
/// Search criteria for transactions
/// </summary>
public class TransactionSearchCriteria
{
    public Guid? AccountId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public TransactionType? Type { get; set; }
    public TransactionStatus? Status { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public string? Description { get; set; }
    public string? Reference { get; set; }
    public Guid? FromAccountId { get; set; }
    public Guid? ToAccountId { get; set; }
}
