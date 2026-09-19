namespace Bank.Payments.Domain.Entities;

/// <summary>
/// Represents a payment in the Payments bounded context
/// Owned by Payments module - responsible for payment lifecycle
/// Communicates with Core Banking via ICoreBankingPostingContract
/// </summary>
public class Payment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    public Guid FromAccountId { get; set; }
    public Guid BeneficiaryId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string? Description { get; set; }
    public string IdempotencyKey { get; set; } = string.Empty;
    
    // Status tracking
    public PaymentStatus Status { get; set; } = PaymentStatus.Created;
    public DateTime RequestedAtUtc { get; set; }
    public DateTime? ProcessedAtUtc { get; set; }
    public DateTime? PostedAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
    
    // Failure tracking
    public string? FailureReason { get; set; }
    public int RetryCount { get; set; }
    public DateTime? LastRetryAtUtc { get; set; }
    
    // Core Banking reference
    public string? PostingReference { get; set; }
    public string? TransactionReference { get; set; }
    
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }

    private Payment() { }

    public static Payment Create(
        Guid customerId,
        Guid fromAccountId,
        Guid beneficiaryId,
        decimal amount,
        string currency,
        string idempotencyKey,
        string? description = null)
    {
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            FromAccountId = fromAccountId,
            BeneficiaryId = beneficiaryId,
            Amount = amount,
            Currency = currency,
            IdempotencyKey = idempotencyKey,
            Description = description,
            Status = PaymentStatus.Created,
            RequestedAtUtc = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow
        };

        return payment;
    }

    public void MarkAsPostingRequested(string reference)
    {
        Status = PaymentStatus.PostingRequested;
        PostingReference = reference;
        ProcessedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void MarkAsPosted(string transactionReference)
    {
        Status = PaymentStatus.Posted;
        TransactionReference = transactionReference;
        PostedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void MarkAsCompleted()
    {
        Status = PaymentStatus.Completed;
        CompletedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void MarkAsFailed(string reason)
    {
        Status = PaymentStatus.Failed;
        FailureReason = reason;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void RecordRetry()
    {
        RetryCount++;
        LastRetryAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public bool CanRetry()
    {
        return Status == PaymentStatus.Failed && RetryCount < 3;
    }
}

public enum PaymentStatus
{
    Created = 1,
    Validated = 2,
    PostingRequested = 3,
    Posted = 4,
    Completed = 5,
    Failed = 6,
    Cancelled = 7
}
