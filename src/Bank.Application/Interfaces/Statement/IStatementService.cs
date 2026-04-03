using Bank.Application.DTOs;
using Bank.Application.DTOs.Statement.Core;
using Bank.Application.DTOs.Statement.Search;
using Bank.Application.DTOs.Statement.Analytics;
using Bank.Application.DTOs.Statement.Delivery;
using Bank.Application.DTOs.Statement.Summary;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for generating and managing account statements
/// </summary>
public interface IStatementService
{
    /// <summary>
    /// Generate a statement for a specific account and period
    /// </summary>
    Task<StatementGenerationResult> GenerateStatementAsync(GenerateStatementRequest request, Guid requestedByUserId);
    
    /// <summary>
    /// Generate consolidated statement for multiple accounts
    /// </summary>
    Task<StatementGenerationResult> GenerateConsolidatedStatementAsync(ConsolidatedStatementRequest request, Guid requestedByUserId);
    
    /// <summary>
    /// Get statement by ID
    /// </summary>
    Task<StatementDto?> GetStatementByIdAsync(Guid statementId);
    
    /// <summary>
    /// Search statements with criteria
    /// </summary>
    Task<StatementSearchResult> SearchStatementsAsync(StatementSearchCriteria criteria);
    
    /// <summary>
    /// Get statements for a specific account
    /// </summary>
    Task<List<StatementDto>> GetAccountStatementsAsync(Guid accountId, int? limit = null);
    
    /// <summary>
    /// Get statement summary for dashboard
    /// </summary>
    Task<StatementSummary> GetStatementSummaryAsync(Guid accountId, DateTime startDate, DateTime endDate);
    
    /// <summary>
    /// Download statement file
    /// </summary>
    Task<(byte[] Content, string FileName, string ContentType)> DownloadStatementAsync(Guid statementId);
    
    /// <summary>
    /// Deliver statement via specified method
    /// </summary>
    Task<bool> DeliverStatementAsync(Guid statementId, StatementDeliveryMethod deliveryMethod, string deliveryAddress);
    
    /// <summary>
    /// Get statement delivery status
    /// </summary>
    Task<StatementDeliveryStatus> GetDeliveryStatusAsync(Guid statementId);
    
    /// <summary>
    /// Regenerate existing statement
    /// </summary>
    Task<StatementGenerationResult> RegenerateStatementAsync(Guid statementId, Guid requestedByUserId);
    
    /// <summary>
    /// Cancel statement generation
    /// </summary>
    Task<bool> CancelStatementGenerationAsync(Guid statementId, Guid cancelledByUserId);
    
    /// <summary>
    /// Get available statement templates
    /// </summary>
    Task<List<StatementTemplate>> GetAvailableTemplatesAsync();
    
    /// <summary>
    /// Validate statement request
    /// </summary>
    Task<(bool IsValid, List<string> Errors)> ValidateStatementRequestAsync(GenerateStatementRequest request);
}