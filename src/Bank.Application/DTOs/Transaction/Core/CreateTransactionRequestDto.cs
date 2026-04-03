using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Transaction.Core;

/// <summary>
/// Request to create a new transaction
/// </summary>
public class CreateTransactionRequest
{
    public Guid FromAccountId { get; set; }
    public Guid ToAccountId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Reference { get; set; }
    public TransactionType Type { get; set; }
}
