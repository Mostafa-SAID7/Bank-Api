using Bank.Application.Interfaces;
using Bank.Application.Services;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Bank.Tests;

public class LoanInterestCalculationTests
{
    private readonly Mock<ILoanRepository> _mockLoanRepository;
    private readonly Mock<IAuditLogService> _mockAuditLogService;
    private readonly Mock<ILogger<LoanInterestCalculationService>> _mockLogger;
    private readonly LoanInterestCalculationService _service;

    public LoanInterestCalculationTests()
    {
        _mockLoanRepository = new Mock<ILoanRepository>();
        _mockAuditLogService = new Mock<IAuditLogService>();
        _mockLogger = new Mock<ILogger<LoanInterestCalculationService>>();
        
        _service = new LoanInterestCalculationService(
            _mockLoanRepository.Object,
            _mockAuditLogService.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task CalculateMonthlyPaymentAsync_ReducingBalance_ReturnsCorrectAmount()
    {
        // Arrange
        var principal = 100000m;
        var annualRate = 10m;
        var termInMonths = 12;
        var method = InterestCalculationMethod.ReducingBalance;

        // Act
        var result = await _service.CalculateMonthlyPaymentAsync(principal, annualRate, termInMonths, method);

        // Assert
        Assert.True(result > 0);
        Assert.True(result > principal / termInMonths); // Should be more than just principal
    }

    [Fact]
    public async Task GetLoanTypeConfigurationAsync_PersonalLoan_ReturnsCorrectConfig()
    {
        // Act
        var config = await _service.GetLoanTypeConfigurationAsync(LoanType.Personal);

        // Assert
        Assert.Equal(LoanType.Personal, config.LoanType);
        Assert.Equal("Personal Loan", config.TypeName);
        Assert.True(config.MinimumAmount > 0);
        Assert.True(config.MaximumAmount > config.MinimumAmount);
        Assert.True(config.BaseInterestRate > 0);
        Assert.NotEmpty(config.RequiredDocuments);
    }

    [Fact]
    public async Task GetInterestRateForLoanTypeAsync_ExcellentCredit_ReturnsLowerRate()
    {
        // Arrange
        var loanType = LoanType.Personal;
        var excellentScore = 800;
        var poorScore = 500;
        var loanAmount = 50000m;

        // Act
        var excellentRate = await _service.GetInterestRateForLoanTypeAsync(loanType, excellentScore, loanAmount);
        var poorRate = await _service.GetInterestRateForLoanTypeAsync(loanType, poorScore, loanAmount);

        // Assert
        Assert.True(excellentRate < poorRate);
        Assert.True(excellentRate > 0);
        Assert.True(poorRate > 0);
    }

    [Fact]
    public async Task CalculateLoanInterestAsync_ValidLoan_ReturnsCalculationResult()
    {
        // Arrange
        var loan = new Loan
        {
            Id = Guid.NewGuid(),
            PrincipalAmount = 100000m,
            InterestRate = 10m,
            TermInMonths = 12,
            OutstandingBalance = 50000m,
            CalculationMethod = InterestCalculationMethod.ReducingBalance,
            DisbursementDate = DateTime.UtcNow.AddMonths(-6)
        };
        var paymentAmount = 5000m;

        // Act
        var result = await _service.CalculateLoanInterestAsync(loan, paymentAmount);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(paymentAmount, result.TotalPayment);
        Assert.True(result.InterestAmount >= 0);
        Assert.True(result.PrincipalAmount >= 0);
        Assert.Equal(paymentAmount, result.InterestAmount + result.PrincipalAmount);
    }

    [Theory]
    [InlineData(LoanType.Personal, 12.0)]
    [InlineData(LoanType.Auto, 8.0)]
    [InlineData(LoanType.Mortgage, 6.0)]
    [InlineData(LoanType.Business, 10.0)]
    public async Task GetLoanTypeConfigurationAsync_DifferentTypes_ReturnsExpectedBaseRates(LoanType loanType, decimal expectedBaseRate)
    {
        // Act
        var config = await _service.GetLoanTypeConfigurationAsync(loanType);

        // Assert
        Assert.Equal(expectedBaseRate, config.BaseInterestRate);
    }
}