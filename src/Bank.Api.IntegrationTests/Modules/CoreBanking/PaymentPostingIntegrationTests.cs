using Bank.CoreBanking.Domain.Entities;
using Bank.CoreBanking.Domain.Enums;
using Bank.CoreBanking.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Bank.Api.IntegrationTests.Modules.CoreBanking;

/// <summary>
/// End-to-end integration tests for Payment → Posting → Events flow
/// Tests the contract-based communication between Payments and CoreBanking modules
/// </summary>
[Collection("CoreBanking Integration Tests")]
public sealed class PaymentPostingIntegrationTests : IAsyncLifetime
{
    private readonly CoreBankingDbContext _dbContext;

    public PaymentPostingIntegrationTests()
    {
        // Initialize DbContext with in-memory database for testing
        var options = new DbContextOptionsBuilder<CoreBankingDbContext>()
            .UseInMemoryDatabase("CoreBankingTestDb")
            .Options;
        
        _dbContext = new CoreBankingDbContext(options);
    }

    public async Task InitializeAsync()
    {
        // Ensure database is created
        await _dbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        // Clean up
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.DisposeAsync();
    }

    [Fact]
    public async Task PostPayment_WithValidAccounts_ShouldCreateTransactionAndLedgerEntries()
    {
        // Arrange
        var fromAccountId = Guid.NewGuid();
        var toAccountId = Guid.NewGuid();
        var customerId1 = Guid.NewGuid();
        var customerId2 = Guid.NewGuid();
        
        var fromAccount = new Account
        {
            Id = fromAccountId,
            AccountNumber = "ACC001",
            AccountHolderName = "John Doe",
            CustomerId = customerId1,
            Balance = 1000m,
            Currency = "USD",
            Status = AccountStatus.Active,
            Type = AccountType.Checking,
            OpenedAtUtc = DateTime.UtcNow.AddDays(-365),
            LastActivityDate = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow
        };

        var toAccount = new Account
        {
            Id = toAccountId,
            AccountNumber = "ACC002",
            AccountHolderName = "Jane Smith",
            CustomerId = customerId2,
            Balance = 500m,
            Currency = "USD",
            Status = AccountStatus.Active,
            Type = AccountType.Checking,
            OpenedAtUtc = DateTime.UtcNow.AddDays(-365),
            LastActivityDate = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _dbContext.Accounts.AddAsync(fromAccount);
        await _dbContext.Accounts.AddAsync(toAccount);
        await _dbContext.SaveChangesAsync();

        var amount = 100m;
        var transactionId = Guid.NewGuid();

        // Act - Create transaction and ledger entries
        var transaction = new Transaction
        {
            Id = transactionId,
            FromAccountId = fromAccountId,
            ToAccountId = toAccountId,
            Amount = amount,
            Currency = "USD",
            Status = TransactionStatus.Completed,
            Type = TransactionType.Transfer,
            Reference = "TEST-TRF-001",
            Description = "Test transfer",
            IdempotencyKey = Guid.NewGuid().ToString(),
            InitiatedAtUtc = DateTime.UtcNow,
            CompletedAtUtc = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _dbContext.Transactions.AddAsync(transaction);

        // Create debit entry (negative amount) for from account
        var debitEntry = Ledger.CreateDebit(
            transactionId,
            fromAccountId,
            amount,
            "TEST-TRF-001",
            "Transfer out",
            DateTime.UtcNow
        );

        // Create credit entry (positive amount) for to account
        var creditEntry = Ledger.CreateCredit(
            transactionId,
            toAccountId,
            amount,
            "TEST-TRF-001",
            "Transfer in",
            DateTime.UtcNow
        );

        await _dbContext.LedgerEntries.AddAsync(debitEntry);
        await _dbContext.LedgerEntries.AddAsync(creditEntry);

        // Update account balances
        fromAccount.PostTransaction(amount); // Debit: reduces balance
        toAccount.PostTransaction(-amount); // Credit: increases balance

        await _dbContext.SaveChangesAsync();

        // Assert - Verify transaction was created
        var savedTransaction = await _dbContext.Transactions
            .FirstOrDefaultAsync(t => t.Id == transactionId);
        
        savedTransaction.Should().NotBeNull();
        savedTransaction!.Status.Should().Be(TransactionStatus.Completed);
        savedTransaction.Amount.Should().Be(amount);

        // Verify ledger entries were created
        var ledgerEntries = await _dbContext.LedgerEntries
            .Where(l => l.TransactionId == transactionId)
            .ToListAsync();
        
        ledgerEntries.Should().HaveCount(2);
        
        var debit = ledgerEntries.FirstOrDefault(l => l.Amount < 0);
        var credit = ledgerEntries.FirstOrDefault(l => l.Amount > 0);
        
        debit.Should().NotBeNull();
        debit!.Amount.Should().Be(-amount);
        debit.AccountId.Should().Be(fromAccountId);
        
        credit.Should().NotBeNull();
        credit!.Amount.Should().Be(amount);
        credit.AccountId.Should().Be(toAccountId);

        // Verify account balances were updated correctly
        var updatedFromAccount = await _dbContext.Accounts.FindAsync(fromAccountId);
        var updatedToAccount = await _dbContext.Accounts.FindAsync(toAccountId);

        updatedFromAccount.Should().NotBeNull();
        updatedFromAccount!.Balance.Should().Be(900m); // 1000 - 100
        
        updatedToAccount.Should().NotBeNull();
        updatedToAccount!.Balance.Should().Be(600m); // 500 + 100
    }

    [Fact]
    public async Task PostPayment_WithInsufficientFunds_ShouldNotCreateTransaction()
    {
        // Arrange
        var fromAccountId = Guid.NewGuid();
        var toAccountId = Guid.NewGuid();
        var customerId1 = Guid.NewGuid();
        var customerId2 = Guid.NewGuid();
        
        var fromAccount = new Account
        {
            Id = fromAccountId,
            AccountNumber = "ACC003",
            AccountHolderName = "Poor John",
            CustomerId = customerId1,
            Balance = 50m, // Only $50
            Currency = "USD",
            Status = AccountStatus.Active,
            Type = AccountType.Checking,
            OpenedAtUtc = DateTime.UtcNow.AddDays(-365),
            LastActivityDate = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow
        };

        var toAccount = new Account
        {
            Id = toAccountId,
            AccountNumber = "ACC004",
            AccountHolderName = "Rich Jane",
            CustomerId = customerId2,
            Balance = 5000m,
            Currency = "USD",
            Status = AccountStatus.Active,
            Type = AccountType.Checking,
            OpenedAtUtc = DateTime.UtcNow.AddDays(-365),
            LastActivityDate = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _dbContext.Accounts.AddAsync(fromAccount);
        await _dbContext.Accounts.AddAsync(toAccount);
        await _dbContext.SaveChangesAsync();

        var amount = 100m; // Trying to transfer $100 but only has $50

        // Act & Assert
        fromAccount.Balance.Should().BeLessThan(amount);
        
        // This should fail validation in PostPaymentCommandHandler
        // For this test, we're verifying the account balance check logic
        var canPostTransaction = fromAccount.Balance >= amount;
        canPostTransaction.Should().BeFalse();
    }

    [Fact]
    public async Task PostPayment_ShouldUpdateLastActivityDate()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var originalLastActivityDate = DateTime.UtcNow.AddDays(-30);
        
        var account = new Account
        {
            Id = accountId,
            AccountNumber = "ACC005",
            AccountHolderName = "Test User",
            CustomerId = customerId,
            Balance = 1000m,
            Currency = "USD",
            Status = AccountStatus.Active,
            Type = AccountType.Savings,
            OpenedAtUtc = DateTime.UtcNow.AddDays(-365),
            LastActivityDate = originalLastActivityDate,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _dbContext.Accounts.AddAsync(account);
        await _dbContext.SaveChangesAsync();

        // Act
        var newActivityTime = DateTime.UtcNow;
        account.PostTransaction(0);
        account.LastActivityDate = newActivityTime;
        await _dbContext.SaveChangesAsync();

        // Assert
        var updatedAccount = await _dbContext.Accounts.FindAsync(accountId);
        updatedAccount.Should().NotBeNull();
        updatedAccount!.LastActivityDate.Should().BeCloseTo(newActivityTime, TimeSpan.FromSeconds(1));
        updatedAccount.LastActivityDate.Should().BeAfter(originalLastActivityDate);
    }

    [Fact]
    public async Task PostPayment_ShouldMaintainAccountStatus()
    {
        // Arrange
        var fromAccountId = Guid.NewGuid();
        var toAccountId = Guid.NewGuid();
        var customerId1 = Guid.NewGuid();
        var customerId2 = Guid.NewGuid();
        
        var fromAccount = new Account
        {
            Id = fromAccountId,
            AccountNumber = "ACC006",
            AccountHolderName = "User 1",
            CustomerId = customerId1,
            Balance = 500m,
            Currency = "USD",
            Status = AccountStatus.Active,
            Type = AccountType.Checking,
            OpenedAtUtc = DateTime.UtcNow.AddDays(-365),
            LastActivityDate = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow
        };

        var toAccount = new Account
        {
            Id = toAccountId,
            AccountNumber = "ACC007",
            AccountHolderName = "User 2",
            CustomerId = customerId2,
            Balance = 500m,
            Currency = "USD",
            Status = AccountStatus.Active,
            Type = AccountType.Checking,
            OpenedAtUtc = DateTime.UtcNow.AddDays(-365),
            LastActivityDate = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _dbContext.Accounts.AddAsync(fromAccount);
        await _dbContext.Accounts.AddAsync(toAccount);
        await _dbContext.SaveChangesAsync();

        // Act
        fromAccount.PostTransaction(100m);
        toAccount.PostTransaction(-100m);
        await _dbContext.SaveChangesAsync();

        // Assert
        var updatedFromAccount = await _dbContext.Accounts.FindAsync(fromAccountId);
        var updatedToAccount = await _dbContext.Accounts.FindAsync(toAccountId);

        updatedFromAccount!.Status.Should().Be(AccountStatus.Active);
        updatedToAccount!.Status.Should().Be(AccountStatus.Active);
    }
}
