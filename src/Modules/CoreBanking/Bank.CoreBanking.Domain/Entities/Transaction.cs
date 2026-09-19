using Bank.CoreBanking.Domain.Enums;

namespace Bank.CoreBanking.Domain.Entities;

/// <summary>
/// Transaction aggregate - represents a movement of funds between accounts
/// Responsible for coordinating double-entry ledger posts
/// </summary>
public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid FromAccountId { get; set; }
    public Guid ToAccountId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string Description { get; set; } = string.Empty;
    public string? Reference { get; set; }
    public string IdempotencyKey { get; set; } = string.Empty;
    
    // Status tracking
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
    public TransactionType Type { get; set; } = TransactionType.Transfer;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? PostedAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
    
    // Failure tracking
    public string? FailureReason { get; set; }
    public int RetryCount { get; set; }
    
    // Concurrency
    public byte[]? RowVersion { get; set; }

    private Transaction() { }

    public static Transaction Create(
        Guid fromAccountId,
        Guid toAccountId,
        decimal amount,
        string currency,
        string description,
        string idempotencyKey,
        TransactionType type = TransactionType.Transfer)
    {
        return new Transaction
        {
            Id = Guid.NewGuid(),
            FromAccountId = fromAccountId,
            ToAccountId = toAccountId,
            Amount = amount,
            Currency = currency,
            Description = description,
            IdempotencyKey = idempotencyKey,
            Type = type,
            Status = TransactionStatus.Pending,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public void MarkAsPosted(string reference)
    {
        Status = TransactionStatus.Posted;
        Reference = reference;
        PostedAtUtc = DateTime.UtcNow;
    }

    public void MarkAsCompleted()
    {
        Status = TransactionStatus.Completed;
        CompletedAtUtc = DateTime.UtcNow;
    }

    public void MarkAsFailed(string reason)
    {
        Status = TransactionStatus.Failed;
        FailureReason = reason;
    }

    public void RecordRetry()
    {
        RetryCount++;
    }

    public bool CanRetry()
    {
        return Status == TransactionStatus.Failed && RetryCount < 3;
    }
}
