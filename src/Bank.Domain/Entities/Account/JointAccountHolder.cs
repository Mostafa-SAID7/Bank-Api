using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

public class JointAccountHolder : BaseEntity
{
    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public JointAccountRole Role { get; set; }
    public DateTime AddedDate { get; set; } = DateTime.UtcNow;
    public DateTime? RemovedDate { get; set; }
    public bool IsActive => RemovedDate == null;
    
    public Guid AddedByUserId { get; set; }
    public User AddedByUser { get; set; } = null!;
    public Guid? RemovedByUserId { get; set; }
    public User? RemovedByUser { get; set; }
    
    // Signature requirements
    public bool RequiresSignature { get; set; } = true;
    public decimal? TransactionLimit { get; set; } // Individual transaction limit
    public decimal? DailyLimit { get; set; } // Daily transaction limit
    
    public string? Notes { get; set; }
    
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
    
    public bool CanPerformTransaction(decimal amount)
    {
        if (!IsActive) return false;
        
        if (Role == JointAccountRole.ViewOnly) return false;
        
        if (TransactionLimit.HasValue && amount > TransactionLimit.Value)
            return false;
            
        return true;
    }
}
