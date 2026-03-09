using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents a maturity notice sent to customers for fixed deposits
/// </summary>
public class MaturityNotice : BaseEntity
{
    public Guid FixedDepositId { get; set; }
    public string NoticeNumber { get; set; } = string.Empty;
    public MaturityNoticeType NoticeType { get; set; }
    public DateTime NoticeDate { get; set; }
    public DateTime MaturityDate { get; set; }
    public NotificationStatus Status { get; set; }
    
    // Notice content
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? TemplateUsed { get; set; }
    
    // Delivery details
    public NotificationChannel DeliveryChannel { get; set; }
    public string? DeliveryAddress { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? DeliveryReference { get; set; }
    public int DeliveryAttempts { get; set; }
    
    // Customer response
    public DateTime? CustomerResponseDate { get; set; }
    public MaturityAction? CustomerChoice { get; set; }
    public string? CustomerInstructions { get; set; }
    public bool ConsentReceived { get; set; }
    
    // Processing details
    public Guid? GeneratedByUserId { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public string? ProcessingNotes { get; set; }
    
    // Follow-up tracking
    public DateTime? FollowUpDate { get; set; }
    public bool RequiresFollowUp { get; set; }
    public int RemindersSent { get; set; }
    
    // Navigation properties
    public virtual FixedDeposit FixedDeposit { get; set; } = null!;
    public virtual User? GeneratedByUser { get; set; }
    
    /// <summary>
    /// Generates a unique notice number
    /// </summary>
    public void GenerateNoticeNumber()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd");
        var random = new Random().Next(1000, 9999);
        var typeCode = NoticeType switch
        {
            MaturityNoticeType.Initial => "IN",
            MaturityNoticeType.Reminder => "RM",
            MaturityNoticeType.Final => "FN",
            MaturityNoticeType.AutoRenewal => "AR",
            _ => "GN"
        };
        NoticeNumber = $"MN{typeCode}{timestamp}{random}";
    }
    
    /// <summary>
    /// Records customer response to the notice
    /// </summary>
    public void RecordCustomerResponse(MaturityAction choice, string? instructions = null)
    {
        CustomerResponseDate = DateTime.UtcNow;
        CustomerChoice = choice;
        CustomerInstructions = instructions;
        ConsentReceived = true;
        Status = NotificationStatus.Read;
    }
    
    /// <summary>
    /// Marks notice as delivered
    /// </summary>
    public void MarkAsDelivered(string deliveryReference)
    {
        Status = NotificationStatus.Delivered;
        DeliveryDate = DateTime.UtcNow;
        DeliveryReference = deliveryReference;
    }
    
    /// <summary>
    /// Records delivery failure
    /// </summary>
    public void RecordDeliveryFailure()
    {
        DeliveryAttempts++;
        Status = NotificationStatus.Failed;
        
        // Set follow-up if max attempts not reached
        if (DeliveryAttempts < 3)
        {
            RequiresFollowUp = true;
            FollowUpDate = DateTime.UtcNow.AddDays(1);
        }
    }
}

/// <summary>
/// Types of maturity notices
/// </summary>
public enum MaturityNoticeType
{
    Initial = 1,         // Initial maturity notice
    Reminder = 2,        // Reminder notice
    Final = 3,           // Final notice before auto-action
    AutoRenewal = 4,     // Auto-renewal confirmation
    MaturityConfirmation = 5 // Maturity processing confirmation
}