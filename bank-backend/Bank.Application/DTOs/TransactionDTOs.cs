using Bank.Domain.Enums;

namespace Bank.Application.DTOs;

public class TransactionDto
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType TransactionType { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public TransactionStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
}

public class CreateTransactionRequest
{
    public Guid FromAccountId { get; set; }
    public Guid ToAccountId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Reference { get; set; }
    public TransactionType Type { get; set; }
}

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

public class TransactionSearchRequest
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
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}