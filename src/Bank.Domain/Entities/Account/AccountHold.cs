using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities.Account;

public class AccountHold : BaseEntity
{
    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
    
    public AccountHoldType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal? Amount { get; set; } // Null means entire account is on hold
    public DateTime PlacedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiryDate { get; set; }
    public DateTime? ReleasedDate { get; set; }
    
    public Guid PlacedByUserId { get; set; }
    public User PlacedByUser { get; set; } = null!;
    public Guid? ReleasedByUserId { get; set; }
    public User? ReleasedByUser { get; set; }
    
    public string? ReferenceNumber { get; set; } // Court order number, compliance case number, etc.
    public string? Notes { get; set; }
    
    public bool IsActive => ReleasedDate == null && (ExpiryDate == null || ExpiryDate > DateTime.UtcNow);
    
    public void Release(Guid releasedByUserId, string? notes = null)
    {
        if (IsActive)
        {
            ReleasedDate = DateTime.UtcNow;
            ReleasedByUserId = releasedByUserId;
            if (!string.IsNullOrEmpty(notes))
            {
                Notes = string.IsNullOrEmpty(Notes) ? notes : $"{Notes}; Released: {notes}";
            }
        }
    }
}