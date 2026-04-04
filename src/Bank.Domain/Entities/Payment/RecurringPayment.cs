using Bank.Domain.Common;
using Bank.Domain.Enums;
using AccountEntity = Bank.Domain.Entities.Account;

namespace Bank.Domain.Entities;

public class RecurringPayment : BaseEntity
{
    public Guid BeneficiaryId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid FromAccountId { get; set; }
    public AccountEntity FromAccount { get; set; } = null!;
    
    public Guid ToAccountId { get; set; }
    public AccountEntity ToAccount { get; set; } = null!;
    
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Reference { get; set; }
    
    // Scheduling
    public RecurringPaymentFrequency Frequency { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? MaxOccurrences { get; set; }
    public DateTime NextExecutionDate { get; set; }
    
    // Status and tracking
    public RecurringPaymentStatus Status { get; set; } = RecurringPaymentStatus.Active;
    public int ExecutionCount { get; set; } = 0;
    public DateTime? LastExecutionDate { get; set; }
    public DateTime? PausedDate { get; set; }
    public string? PauseReason { get; set; }
    
    // Failure handling
    public int FailureCount { get; set; } = 0;
    public int MaxRetries { get; set; } = 3;
    public string? LastFailureReason { get; set; }
    
    // Created by
    public Guid CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = null!;
    
    // Navigation properties
    public ICollection<RecurringPaymentExecution> Executions { get; set; } = new List<RecurringPaymentExecution>();
    
    public DateTime CalculateNextExecutionDate()
    {
        var baseDate = LastExecutionDate ?? StartDate;
        
        return Frequency switch
        {
            RecurringPaymentFrequency.Daily => baseDate.AddDays(1),
            RecurringPaymentFrequency.Weekly => baseDate.AddDays(7),
            RecurringPaymentFrequency.BiWeekly => baseDate.AddDays(14),
            RecurringPaymentFrequency.Monthly => baseDate.AddMonths(1),
            RecurringPaymentFrequency.Quarterly => baseDate.AddMonths(3),
            RecurringPaymentFrequency.SemiAnnually => baseDate.AddMonths(6),
            RecurringPaymentFrequency.Annually => baseDate.AddYears(1),
            _ => baseDate.AddMonths(1)
        };
    }
    
    public bool ShouldExecute()
    {
        if (Status != RecurringPaymentStatus.Active)
            return false;
            
        if (DateTime.UtcNow < NextExecutionDate)
            return false;
            
        if (EndDate.HasValue && DateTime.UtcNow > EndDate.Value)
            return false;
            
        if (MaxOccurrences.HasValue && ExecutionCount >= MaxOccurrences.Value)
            return false;
            
        return true;
    }
    
    public void Pause(string reason)
    {
        Status = RecurringPaymentStatus.Paused;
        PausedDate = DateTime.UtcNow;
        PauseReason = reason;
    }
    
    public void Resume()
    {
        Status = RecurringPaymentStatus.Active;
        PausedDate = null;
        PauseReason = null;
        NextExecutionDate = CalculateNextExecutionDate();
    }
    
    public void Cancel(string reason)
    {
        Status = RecurringPaymentStatus.Cancelled;
        PauseReason = reason;
    }
    
    public void RecordExecution(bool success, string? failureReason = null)
    {
        if (success)
        {
            ExecutionCount++;
            LastExecutionDate = DateTime.UtcNow;
            NextExecutionDate = CalculateNextExecutionDate();
            FailureCount = 0; // Reset failure count on success
            LastFailureReason = null;
        }
        else
        {
            FailureCount++;
            LastFailureReason = failureReason;
            
            if (FailureCount >= MaxRetries)
            {
                Status = RecurringPaymentStatus.Failed;
            }
        }
    }
}

public class RecurringPaymentExecution : BaseEntity
{
    public Guid RecurringPaymentId { get; set; }
    public RecurringPayment RecurringPayment { get; set; } = null!;
    
    public DateTime ScheduledDate { get; set; }
    public DateTime? ExecutedDate { get; set; }
    public decimal Amount { get; set; }
    public RecurringPaymentExecutionStatus Status { get; set; }
    
    public Guid? TransactionId { get; set; }
    public Transaction? Transaction { get; set; }
    
    public string? FailureReason { get; set; }
    public int RetryCount { get; set; } = 0;
}
