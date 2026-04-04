using Bank.Domain.Common;
using Bank.Domain.Enums;
using AccountEntity = Bank.Domain.Entities.Account;

namespace Bank.Domain.Entities;

public class Transaction : BaseEntity
{
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Reference { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
    public TransactionType Type { get; set; }
    
    public Guid FromAccountId { get; set; }
    public AccountEntity FromAccount { get; set; } = null!;
    
    public Guid ToAccountId { get; set; }
    public AccountEntity ToAccount { get; set; } = null!;

    public Guid? BatchJobId { get; set; }
    public BatchJob? BatchJob { get; set; }
}
