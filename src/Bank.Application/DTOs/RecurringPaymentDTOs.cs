using Bank.Domain.Enums;

namespace Bank.Application.DTOs;

public class CreateRecurringPaymentRequest
{
    public Guid FromAccountId { get; set; }
    public Guid ToAccountId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Reference { get; set; }
    public RecurringPaymentFrequency Frequency { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? MaxOccurrences { get; set; }
    public int MaxRetries { get; set; } = 3;
    public Guid CreatedByUserId { get; set; }
}

public class UpdateRecurringPaymentRequest
{
    public decimal? Amount { get; set; }
    public string? Description { get; set; }
    public string? Reference { get; set; }
    public RecurringPaymentFrequency? Frequency { get; set; }
    public DateTime? EndDate { get; set; }
    public int? MaxOccurrences { get; set; }
    public int? MaxRetries { get; set; }
}

public class BulkTransferRequest
{
    public Guid FromAccountId { get; set; }
    public List<BulkTransferItem> Transfers { get; set; } = new();
    public string Description { get; set; } = string.Empty;
    public Guid CreatedByUserId { get; set; }
}

public class BulkTransferItem
{
    public Guid ToAccountId { get; set; }
    public decimal Amount { get; set; }
    public string? Reference { get; set; }
    public string? Description { get; set; }
}

public class BulkTransferResult
{
    public Guid BatchId { get; set; }
    public BulkTransferStatus Status { get; set; }
    public int TotalTransfers { get; set; }
    public int SuccessfulTransfers { get; set; }
    public int FailedTransfers { get; set; }
    public decimal TotalAmount { get; set; }
    public List<BulkTransferItemResult> Results { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}

public class BulkTransferItemResult
{
    public Guid ToAccountId { get; set; }
    public decimal Amount { get; set; }
    public string? Reference { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public Guid? TransactionId { get; set; }
}