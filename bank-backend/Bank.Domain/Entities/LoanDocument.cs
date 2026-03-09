using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents a document associated with a loan
/// </summary>
public class LoanDocument : BaseEntity
{
    public Guid LoanId { get; set; }
    public Loan Loan { get; set; } = null!;
    
    public LoanDocumentType DocumentType { get; set; }
    public string DocumentName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    
    public bool IsRequired { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? VerifiedDate { get; set; }
    public Guid? VerifiedBy { get; set; }
    public User? VerifiedByUser { get; set; }
    
    public string? Description { get; set; }
    public string? VerificationNotes { get; set; }
    
    // Domain methods
    public bool IsImage()
    {
        return ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
    }
    
    public bool IsPdf()
    {
        return ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase);
    }
    
    public void MarkAsVerified(Guid verifiedBy, string? notes = null)
    {
        IsVerified = true;
        VerifiedDate = DateTime.UtcNow;
        VerifiedBy = verifiedBy;
        VerificationNotes = notes;
    }
}