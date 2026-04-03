using Bank.Application.DTOs;
using Bank.Application.Services;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Bank.Tests;

/// <summary>
/// Tests for enhanced statement generation features (Task 8.2)
/// </summary>
public class StatementGenerationTests
{
    private readonly Mock<ILogger<StatementGenerator>> _mockLogger;
    private readonly StatementGenerator _statementGenerator;

    public StatementGenerationTests()
    {
        _mockLogger = new Mock<ILogger<StatementGenerator>>();
        _statementGenerator = new StatementGenerator(_mockLogger.Object);
    }

    [Fact]
    public async Task GenerateHtmlStatement_WithAdvancedFeatures_IncludesStatisticsAndFees()
    {
        // Arrange
        var statement = CreateTestStatement();
        var template = new StatementTemplate
        {
            IncludeBankBranding = true,
            IncludeRegulatoryDisclosures = true
        };

        // Act
        var (content, fileName) = await _statementGenerator.GenerateHtmlStatementAsync(statement, template);
        var htmlContent = System.Text.Encoding.UTF8.GetString(content);

        // Assert
        Assert.Contains("Monthly Statistics", htmlContent);
        Assert.Contains("Fee Summary", htmlContent);
        Assert.Contains("Important Disclosures", htmlContent);
        Assert.Contains("Average Monthly Credits", htmlContent);
        Assert.Contains("Largest Credit", htmlContent);
        Assert.Contains("Days with Activity", htmlContent);
        Assert.Contains("FDIC Insured", htmlContent);
        Assert.Contains("Privacy Notice", htmlContent);
    }

    [Fact]
    public async Task GenerateConsolidatedStatement_WithMultipleAccounts_CombinesData()
    {
        // Arrange
        var statements = new List<AccountStatement>
        {
            CreateTestStatement(),
            CreateTestStatement()
        };
        
        var request = new ConsolidatedStatementRequest
        {
            Format = StatementFormat.HTML,
            IncludeAccountSummaries = true,
            IncludeConsolidatedSummary = true,
            IncludeTransactionDetails = true
        };

        // Act
        var (content, fileName) = await _statementGenerator.GenerateConsolidatedStatementAsync(statements, request);
        var htmlContent = System.Text.Encoding.UTF8.GetString(content);

        // Assert
        Assert.Contains("Consolidated Account Statement", htmlContent);
        Assert.Contains("Account Summaries", htmlContent);
        Assert.Contains("Consolidated Summary", htmlContent);
        Assert.Contains("Total Opening Balance", htmlContent);
        Assert.Contains("Total Closing Balance", htmlContent);
    }

    [Fact]
    public void CalculateStatistics_WithTransactions_ComputesCorrectValues()
    {
        // Arrange
        var statement = CreateTestStatement();

        // Act
        statement.CalculateStatistics();

        // Assert
        Assert.True(statement.TotalTransactions > 0);
        Assert.True(statement.TotalCredits > 0);
        Assert.True(statement.TotalDebits > 0);
        Assert.True(statement.MinimumBalance <= statement.MaximumBalance);
        Assert.True(statement.AverageBalance > 0);
    }

    private AccountStatement CreateTestStatement()
    {
        var account = new Account
        {
            Id = Guid.NewGuid(),
            AccountNumber = "1234567890",
            AccountHolderName = "Test User",
            Type = AccountType.Checking,
            Balance = 5000m
        };

        var statement = new AccountStatement
        {
            Id = Guid.NewGuid(),
            AccountId = account.Id,
            Account = account,
            StatementDate = DateTime.UtcNow,
            PeriodStartDate = DateTime.UtcNow.AddMonths(-1),
            PeriodEndDate = DateTime.UtcNow,
            StatementNumber = "STMT-202412-7890-001",
            OpeningBalance = 4000m,
            ClosingBalance = 5000m,
            Transactions = new List<StatementTransaction>()
        };

        // Add sample transactions
        var transactions = new List<StatementTransaction>
        {
            new StatementTransaction
            {
                Id = Guid.NewGuid(),
                StatementId = statement.Id,
                TransactionDate = DateTime.UtcNow.AddDays(-20),
                Description = "Salary Deposit",
                Amount = 3000m,
                RunningBalance = 7000m,
                Type = TransactionType.ACH,
                Category = "Income"
            },
            new StatementTransaction
            {
                Id = Guid.NewGuid(),
                StatementId = statement.Id,
                TransactionDate = DateTime.UtcNow.AddDays(-15),
                Description = "Grocery Store Purchase",
                Amount = -150m,
                RunningBalance = 6850m,
                Type = TransactionType.ACH,
                Category = "Food & Dining"
            },
            new StatementTransaction
            {
                Id = Guid.NewGuid(),
                StatementId = statement.Id,
                TransactionDate = DateTime.UtcNow.AddDays(-10),
                Description = "Monthly Maintenance Fee",
                Amount = -25m,
                RunningBalance = 6825m,
                Type = TransactionType.ACH,
                Category = "Fees & Charges"
            },
            new StatementTransaction
            {
                Id = Guid.NewGuid(),
                StatementId = statement.Id,
                TransactionDate = DateTime.UtcNow.AddDays(-5),
                Description = "Interest Earned",
                Amount = 15m,
                RunningBalance = 6840m,
                Type = TransactionType.ACH,
                Category = "Interest"
            }
        };

        statement.Transactions = transactions;
        
        // Calculate statistics to populate fees and other calculated fields
        statement.CalculateStatistics();
        
        return statement;
    }
}