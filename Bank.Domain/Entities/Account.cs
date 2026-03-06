using Bank.Domain.Common;

namespace Bank.Domain.Entities;

public class Account : BaseEntity
{
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountHolderName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<Transaction> SentTransactions { get; set; } = new List<Transaction>();
    public ICollection<Transaction> ReceivedTransactions { get; set; } = new List<Transaction>();
}
