using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

public class Transaction : BaseEntity
{
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime? ProcessedAt { get; set; }
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
    public TransactionType Type { get; set; }
    
    public Guid FromAccountId { get; set; }
    public Account FromAccount { get; set; } = null!;
    
    public Guid ToAccountId { get; set; }
    public Account ToAccount { get; set; } = null!;

    public Guid? BatchJobId { get; set; }
    public BatchJob? BatchJob { get; set; }
}
