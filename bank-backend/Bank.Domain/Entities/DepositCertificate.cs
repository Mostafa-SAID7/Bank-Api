using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents a deposit certificate issued for fixed deposits
/// </summary>
public class DepositCertificate : BaseEntity
{
    public Guid FixedDepositId { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public DepositCertificateStatus Status { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string DeliveryMethod { get; set; } = string.Empty;
    public string? DeliveryAddress { get; set; }
    public string? DeliveryReference { get; set; }
    
    // Certificate content
    public string CertificateTemplate { get; set; } = string.Empty;
    public string CertificateContent { get; set; } = string.Empty;
    public byte[]? CertificatePdf { get; set; }
    public string? PdfFileName { get; set; }
    
    // Digital signature and security
    public string? DigitalSignature { get; set; }
    public string? SecurityHash { get; set; }
    public DateTime? VerificationDate { get; set; }
    
    // Replacement tracking
    public Guid? ReplacedCertificateId { get; set; }
    public string? ReplacementReason { get; set; }
    public Guid? ReplacedByUserId { get; set; }
    
    // Processing details
    public Guid? GeneratedByUserId { get; set; }
    public Guid? IssuedByUserId { get; set; }
    public string? ProcessingNotes { get; set; }
    
    // Navigation properties
    public virtual FixedDeposit FixedDeposit { get; set; } = null!;
    public virtual DepositCertificate? ReplacedCertificate { get; set; }
    public virtual User? GeneratedByUser { get; set; }
    public virtual User? IssuedByUser { get; set; }
    public virtual User? ReplacedByUser { get; set; }
    
    /// <summary>
    /// Generates a unique certificate number
    /// </summary>
    public void GenerateCertificateNumber()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd");
        var random = new Random().Next(10000, 99999);
        CertificateNumber = $"DC{timestamp}{random}";
    }
    
    /// <summary>
    /// Generates security hash for certificate verification
    /// </summary>
    public void GenerateSecurityHash()
    {
        var content = $"{CertificateNumber}{FixedDepositId}{IssueDate:yyyyMMddHHmmss}";
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(content));
        SecurityHash = Convert.ToBase64String(hashBytes);
    }
    
    /// <summary>
    /// Marks certificate as delivered
    /// </summary>
    public void MarkAsDelivered(string deliveryReference, DateTime? deliveryDate = null)
    {
        Status = DepositCertificateStatus.Delivered;
        DeliveryReference = deliveryReference;
        DeliveryDate = deliveryDate ?? DateTime.UtcNow;
    }
    
    /// <summary>
    /// Cancels the certificate
    /// </summary>
    public void Cancel(string reason, Guid cancelledByUserId)
    {
        Status = DepositCertificateStatus.Cancelled;
        ReplacementReason = reason;
        ReplacedByUserId = cancelledByUserId;
    }
}