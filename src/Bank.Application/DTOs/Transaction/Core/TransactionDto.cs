using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Transaction.Core;

/// <summary>
/// Data transfer object for transaction details
/// </summary>
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
