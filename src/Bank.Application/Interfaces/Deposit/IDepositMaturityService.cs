using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service for managing deposit maturity processing and renewals
/// </summary>
public interface IDepositMaturityService
{
    Task<MaturityProcessingResult> ProcessApproachingMaturityAsync();
    Task<MaturityProcessingResult> ProcessMaturedDepositsAsync();
    Task<bool> ProcessCustomerConsentAsync(Guid depositId, bool consentGiven, MaturityAction? preferredAction = null);
    Task<int> SendRenewalRemindersAsync();
    Task<MaturityProcessingResult> ProcessAutomaticRenewalsAsync();
}

/// <summary>
/// Result of maturity processing operations
/// </summary>
public class MaturityProcessingResult
{
    public int ProcessedCount { get; set; }
    public int NoticesSent { get; set; }
    public int SuccessfulRenewals { get; set; }
    public int MaturityActionsProcessed { get; set; }
    public List<string> Errors { get; set; } = new();
    public bool HasErrors => Errors.Any();
}
