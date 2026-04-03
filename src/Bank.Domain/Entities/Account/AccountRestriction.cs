using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities.Account;

public class AccountRestriction : BaseEntity
{
    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
    
    public AccountRestrictionType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime AppliedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiryDate { get; set; }
    public DateTime? RemovedDate { get; set; }
    
    public Guid AppliedByUserId { get; set; }
    public User AppliedByUser { get; set; } = null!;
    public Guid? RemovedByUserId { get; set; }
    public User? RemovedByUser { get; set; }
    
    // Restriction parameters
    public decimal? DailyLimit { get; set; }
    public decimal? MonthlyLimit { get; set; }
    public int? TransactionCountLimit { get; set; }
    
    public string? Notes { get; set; }
    
    public bool IsActive => RemovedDate == null && (ExpiryDate == null || ExpiryDate > DateTime.UtcNow);
    
    public void Remove(Guid removedByUserId, string? notes = null)
    {
        if (IsActive)
        {
            RemovedDate = DateTime.UtcNow;
            RemovedByUserId = removedByUserId;
            if (!string.IsNullOrEmpty(notes))
            {
                Notes = string.IsNullOrEmpty(Notes) ? notes : $"{Notes}; Removed: {notes}";
            }
        }
    }
}