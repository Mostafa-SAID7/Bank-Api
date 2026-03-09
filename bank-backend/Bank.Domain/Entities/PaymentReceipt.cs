using Bank.Application.DTOs;
using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents a payment receipt for bill payments
/// </summary>
public class PaymentReceipt : BaseEntity
{
    public Guid PaymentId { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string BillerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public DateTime PaymentDate { get; set; }
    public DateTime ProcessedDate { get; set; }
    public string ConfirmationNumber { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public PaymentMethod PaymentMethod { get; set; }
    public decimal? ProcessingFee { get; set; }
    public BillPaymentStatus Status { get; set; }
    public string ReceiptDataJson { get; set; } = string.Empty; // Additional receipt data as JSON

    // Navigation properties
    public virtual BillPayment Payment { get; set; } = null!;
    public virtual User Customer { get; set; } = null!;

    /// <summary>
    /// Generate a unique receipt number
    /// </summary>
    public static string GenerateReceiptNumber()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var random = new Random().Next(1000, 9999);
        return $"RCP-{timestamp}-{random}";
    }

    /// <summary>
    /// Update receipt status
    /// </summary>
    public void UpdateStatus(BillPaymentStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Check if receipt is for a successful payment
    /// </summary>
    public bool IsSuccessfulPayment()
    {
        return Status is BillPaymentStatus.Processed or BillPaymentStatus.Delivered;
    }
}