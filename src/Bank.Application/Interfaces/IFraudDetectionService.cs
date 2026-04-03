using Bank.Application.DTOs;
using Bank.Domain.Entities;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service for detecting fraudulent activities and suspicious patterns
/// </summary>
public interface IFraudDetectionService
{
    /// <summary>
    /// Analyze transaction for fraud patterns
    /// </summary>
    Task<FraudAnalysisResult> AnalyzeTransactionAsync(Transaction transaction, User user);
    
    /// <summary>
    /// Analyze login attempt for suspicious activity
    /// </summary>
    Task<FraudAnalysisResult> AnalyzeLoginAttemptAsync(Guid userId, string ipAddress, string userAgent);
    
    /// <summary>
    /// Check if account should be temporarily frozen
    /// </summary>
    Task<bool> ShouldFreezeAccountAsync(Guid userId);
    
    /// <summary>
    /// Get fraud risk score for user
    /// </summary>
    Task<FraudRiskScore> GetUserRiskScoreAsync(Guid userId);
    
    /// <summary>
    /// Report suspicious activity
    /// </summary>
    Task ReportSuspiciousActivityAsync(SuspiciousActivityReport report);
    
    /// <summary>
    /// Get fraud detection rules
    /// </summary>
    Task<List<FraudRule>> GetActiveRulesAsync();
    
    /// <summary>
    /// Update fraud detection rules
    /// </summary>
    Task UpdateRuleAsync(FraudRule rule);
}