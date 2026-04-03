using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities.Account;

public class AccountStatusHistory : BaseEntity
{
    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
    
    public AccountStatus FromStatus { get; set; }
    public AccountStatus ToStatus { get; set; }
    public DateTime ChangedDate { get; set; } = DateTime.UtcNow;
    public string Reason { get; set; } = string.Empty;
    public string? Notes { get; set; }
    
    public Guid ChangedByUserId { get; set; }
    public User ChangedByUser { get; set; } = null!;
    
    // Additional context
    public string? SystemReference { get; set; } // Reference to batch job, compliance case, etc.
    public bool IsSystemGenerated { get; set; } = false;
}