using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents a biller that customers can pay bills to
/// </summary>
public class Biller : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public BillerCategory Category { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public string RoutingNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string SupportedPaymentMethods { get; set; } = string.Empty; // JSON array of payment methods
    public decimal MinAmount { get; set; }
    public decimal MaxAmount { get; set; }
    public int ProcessingDays { get; set; }

    // Navigation properties
    public virtual ICollection<BillPayment> BillPayments { get; set; } = new List<BillPayment>();

    /// <summary>
    /// Check if the biller supports a specific payment method
    /// </summary>
    public bool SupportsPaymentMethod(string paymentMethod)
    {
        if (string.IsNullOrEmpty(SupportedPaymentMethods))
            return false;

        try
        {
            var methods = System.Text.Json.JsonSerializer.Deserialize<string[]>(SupportedPaymentMethods);
            return methods?.Contains(paymentMethod, StringComparer.OrdinalIgnoreCase) == true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Check if the payment amount is within the allowed range
    /// </summary>
    public bool IsAmountValid(decimal amount)
    {
        return amount >= MinAmount && amount <= MaxAmount;
    }

    /// <summary>
    /// Calculate the expected processing date for a payment
    /// </summary>
    public DateTime CalculateProcessingDate(DateTime scheduledDate)
    {
        return scheduledDate.AddDays(ProcessingDays);
    }
}