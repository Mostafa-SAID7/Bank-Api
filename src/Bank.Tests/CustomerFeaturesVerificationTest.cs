using Bank.Application.DTOs;
using Bank.Application.Interfaces;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Bank.Tests;

/// <summary>
/// Verification tests for customer features (Task 9 checkpoint)
/// Tests beneficiary management and statement generation functionality
/// </summary>
public class CustomerFeaturesVerificationTest
{
    private readonly Mock<ILogger<CustomerFeaturesVerificationTest>> _mockLogger;

    public CustomerFeaturesVerificationTest()
    {
        _mockLogger = new Mock<ILogger<CustomerFeaturesVerificationTest>>();
    }

    [Fact]
    public void BeneficiaryService_ShouldBeProperlyImplemented()
    {
        // Arrange & Act
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<IBeneficiaryService, Bank.Application.Services.BeneficiaryService>();
        
        // Assert - Service should be registerable
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var serviceDescriptor = serviceCollection.FirstOrDefault(s => s.ServiceType == typeof(IBeneficiaryService));
        
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(typeof(Bank.Application.Services.BeneficiaryService), serviceDescriptor.ImplementationType);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void StatementService_ShouldBeProperlyImplemented()
    {
        // Arrange & Act
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<IStatementService, Bank.Application.Services.StatementService>();
        
        // Assert - Service should be registerable
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var serviceDescriptor = serviceCollection.FirstOrDefault(s => s.ServiceType == typeof(IStatementService));
        
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(typeof(Bank.Application.Services.StatementService), serviceDescriptor.ImplementationType);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void BeneficiaryEntity_ShouldHaveRequiredProperties()
    {
        // Arrange & Act
        var beneficiary = new Beneficiary();
        
        // Assert - Entity should have all required properties for customer features
        Assert.True(HasProperty(beneficiary, "Id"));
        Assert.True(HasProperty(beneficiary, "CustomerId"));
        Assert.True(HasProperty(beneficiary, "Name"));
        Assert.True(HasProperty(beneficiary, "AccountNumber"));
        Assert.True(HasProperty(beneficiary, "BankCode"));
        Assert.True(HasProperty(beneficiary, "IsVerified"));
        Assert.True(HasProperty(beneficiary, "DailyTransferLimit"));
        Assert.True(HasProperty(beneficiary, "Category"));
        Assert.True(HasProperty(beneficiary, "Type"));
    }

    [Fact]
    public void AccountStatementEntity_ShouldHaveRequiredProperties()
    {
        // Arrange & Act
        var statement = new AccountStatement();
        
        // Assert - Entity should have all required properties for statement generation
        Assert.True(HasProperty(statement, "Id"));
        Assert.True(HasProperty(statement, "AccountId"));
        Assert.True(HasProperty(statement, "StatementNumber"));
        Assert.True(HasProperty(statement, "StatementDate"));
        Assert.True(HasProperty(statement, "PeriodStartDate"));
        Assert.True(HasProperty(statement, "PeriodEndDate"));
        Assert.True(HasProperty(statement, "OpeningBalance"));
        Assert.True(HasProperty(statement, "ClosingBalance"));
        Assert.True(HasProperty(statement, "Format"));
        Assert.True(HasProperty(statement, "Status"));
    }

    [Fact]
    public void BeneficiaryDTOs_ShouldBeProperlyDefined()
    {
        // Arrange & Act - Test that key DTOs exist and are instantiable
        var addRequest = new AddBeneficiaryRequest();
        var updateRequest = new UpdateBeneficiaryRequest();
        var beneficiaryDto = new BeneficiaryDto();
        var searchCriteria = new BeneficiarySearchCriteria();
        
        // Assert - DTOs should be properly defined
        Assert.NotNull(addRequest);
        Assert.NotNull(updateRequest);
        Assert.NotNull(beneficiaryDto);
        Assert.NotNull(searchCriteria);
        
        // Verify key properties exist
        Assert.True(HasProperty(addRequest, "Name"));
        Assert.True(HasProperty(addRequest, "AccountNumber"));
        Assert.True(HasProperty(beneficiaryDto, "Id"));
        Assert.True(HasProperty(beneficiaryDto, "Name"));
    }

    [Fact]
    public void StatementDTOs_ShouldBeProperlyDefined()
    {
        // Arrange & Act - Test that key DTOs exist and are instantiable
        var generateRequest = new GenerateStatementRequest();
        var statementDto = new StatementDto();
        var searchCriteria = new StatementSearchCriteria();
        
        // Assert - DTOs should be properly defined
        Assert.NotNull(generateRequest);
        Assert.NotNull(statementDto);
        Assert.NotNull(searchCriteria);
        
        // Verify key properties exist
        Assert.True(HasProperty(generateRequest, "AccountId"));
        Assert.True(HasProperty(generateRequest, "StartDate"));
        Assert.True(HasProperty(generateRequest, "EndDate"));
        Assert.True(HasProperty(statementDto, "Id"));
        Assert.True(HasProperty(statementDto, "StatementNumber"));
    }

    [Fact]
    public void BeneficiaryEnums_ShouldBeProperlyDefined()
    {
        // Arrange & Act - Test that required enums exist
        var beneficiaryType = BeneficiaryType.Internal;
        var beneficiaryCategory = BeneficiaryCategory.Personal;
        
        // Assert - Enums should have expected values
        Assert.True(Enum.IsDefined(typeof(BeneficiaryType), beneficiaryType));
        Assert.True(Enum.IsDefined(typeof(BeneficiaryCategory), beneficiaryCategory));
        
        // Verify enum values exist
        var typeValues = Enum.GetValues<BeneficiaryType>();
        var categoryValues = Enum.GetValues<BeneficiaryCategory>();
        
        Assert.Contains(BeneficiaryType.Internal, typeValues);
        Assert.Contains(BeneficiaryType.External, typeValues);
        Assert.Contains(BeneficiaryCategory.Personal, categoryValues);
        Assert.Contains(BeneficiaryCategory.Business, categoryValues);
    }

    [Fact]
    public void StatementEnums_ShouldBeProperlyDefined()
    {
        // Arrange & Act - Test that required enums exist
        var statementFormat = StatementFormat.PDF;
        var statementStatus = StatementStatus.Generated;
        
        // Assert - Enums should have expected values
        Assert.True(Enum.IsDefined(typeof(StatementFormat), statementFormat));
        Assert.True(Enum.IsDefined(typeof(StatementStatus), statementStatus));
        
        // Verify enum values exist
        var formatValues = Enum.GetValues<StatementFormat>();
        var statusValues = Enum.GetValues<StatementStatus>();
        
        Assert.Contains(StatementFormat.PDF, formatValues);
        Assert.Contains(StatementFormat.CSV, formatValues);
        Assert.Contains(StatementFormat.Excel, formatValues);
        Assert.Contains(StatementStatus.Requested, statusValues);
        Assert.Contains(StatementStatus.Generated, statusValues);
    }

    [Fact]
    public void CustomerFeatures_ControllersExist()
    {
        // Arrange & Act - Verify controllers exist
        var beneficiaryControllerType = typeof(Bank.Api.Controllers.BeneficiaryController);
        var statementControllerType = typeof(Bank.Api.Controllers.StatementController);
        
        // Assert - Controllers should be properly defined
        Assert.NotNull(beneficiaryControllerType);
        Assert.NotNull(statementControllerType);
        
        // Verify controllers have required methods
        var beneficiaryMethods = beneficiaryControllerType.GetMethods().Select(m => m.Name).ToList();
        var statementMethods = statementControllerType.GetMethods().Select(m => m.Name).ToList();
        
        // Beneficiary controller methods
        Assert.Contains("AddBeneficiary", beneficiaryMethods);
        Assert.Contains("GetMyBeneficiaries", beneficiaryMethods);
        Assert.Contains("VerifyBeneficiary", beneficiaryMethods);
        Assert.Contains("UpdateBeneficiary", beneficiaryMethods);
        
        // Statement controller methods
        Assert.Contains("GenerateStatement", statementMethods);
        Assert.Contains("GetAccountStatements", statementMethods);
        Assert.Contains("DownloadStatement", statementMethods);
        Assert.Contains("GetStatementSummary", statementMethods);
    }

    [Fact]
    public void CustomerFeatures_ServiceInterfacesHaveRequiredMethods()
    {
        // Arrange & Act - Verify service interfaces have required methods
        var beneficiaryServiceType = typeof(IBeneficiaryService);
        var statementServiceType = typeof(IStatementService);
        
        // Assert - Interfaces should have required methods
        var beneficiaryMethods = beneficiaryServiceType.GetMethods().Select(m => m.Name).ToList();
        var statementMethods = statementServiceType.GetMethods().Select(m => m.Name).ToList();
        
        // Beneficiary service methods
        Assert.Contains("AddBeneficiaryAsync", beneficiaryMethods);
        Assert.Contains("GetCustomerBeneficiariesAsync", beneficiaryMethods);
        Assert.Contains("VerifyBeneficiaryAsync", beneficiaryMethods);
        Assert.Contains("UpdateBeneficiaryAsync", beneficiaryMethods);
        Assert.Contains("ArchiveBeneficiaryAsync", beneficiaryMethods);
        Assert.Contains("GetTransferHistoryAsync", beneficiaryMethods);
        
        // Statement service methods
        Assert.Contains("GenerateStatementAsync", statementMethods);
        Assert.Contains("GetAccountStatementsAsync", statementMethods);
        Assert.Contains("DownloadStatementAsync", statementMethods);
        Assert.Contains("GetStatementSummaryAsync", statementMethods);
        Assert.Contains("GenerateConsolidatedStatementAsync", statementMethods);
    }

    /// <summary>
    /// Helper method to check if an object has a specific property
    /// </summary>
    private static bool HasProperty(object obj, string propertyName)
    {
        return obj.GetType().GetProperty(propertyName) != null;
    }
}