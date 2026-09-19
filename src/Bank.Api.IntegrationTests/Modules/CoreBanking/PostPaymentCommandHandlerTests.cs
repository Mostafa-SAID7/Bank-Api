using Bank.CoreBanking.Application;
using Bank.CoreBanking.Application.Handlers;
using Bank.CoreBanking.Domain.Entities;
using Bank.CoreBanking.Domain.Enums;
using Bank.CoreBanking.Infrastructure.Data;
using Bank.CoreBanking.Infrastructure.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Bank.Api.IntegrationTests.Modules.CoreBanking;

/// <summary>
/// Unit and integration tests for PostPaymentCommandHandler
/// Tests the ICoreBankingPostingContract implementation
/// </summary>
[Collection("PostPaymentCommandHandler Tests")]
public sealed class PostPaymentCommandHandlerTests : IAsyncLifetime
{
    private readonly CoreBankingDbContext _dbContext;
    private readonly PostPaymentCommandHandler _handler;

    public PostPaymentCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<CoreBankingDbContext>()
            .UseInMemoryDatabase("PostingHandlerTestDb")
            .Options;
        
        _dbContext = new CoreBankingDbContext(options);
        
        // Initialize repositories
        var accountRepository = new AccountRepository(_dbContext);
        var transactionRepository = new TransactionRepository(_dbContext);
        var ledgerRepository = new LedgerRepository(_dbContext);
        var eventPublisher = new CorePostingEventPublisher(_dbContext);
        
        _handler = new PostPaymentCommandHandler(
            accountRepository,
            transactionRepository,
            ledgerRepository,
            eventPublisher
        );
    }

    public async Task InitializeAsync()
    {
        await _dbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.DisposeAsync();
    }

    [Fact]
    public async Task PostPayment_WithValidPayment_ShouldSucceed()
    {
        // Arrange
        var fromAccountId = Guid.NewGuid();
        var toAccountId = Guid.NewGuid();
        var customerId1 = Guid.NewGuid();
        var customerId2 = Guid.NewGuid();

        var fromAccount = CreateTestAccount(fromAccountId, "ACC001", "Sender", customerId1, 1000m);
        var toAccount = CreateTestAccount(toAccountId, "ACC002", "Receiver", customerId2, 500m);

        await _dbContext.Accounts.AddAsync(fromAccount);
        await _dbContext.Accounts.AddAsync(toAccount);
        await _dbContext.SaveChangesAsync();

        var command = new PostPaymentCommand(
            fromAccountId: fromAccountId,
            toAccountId: toAccountId,
            amount: 250m,
            currency: "USD",
            paymentReference: "PAY-001",
            paymentDescription: "Test payment",
            idempotencyKey: Guid.NewGuid().ToString()
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        // Verify transaction was created
        var transaction = await _dbContext.Transactions
            .FirstOrDefaultAsync(t => t.IdempotencyKey == command.IdempotencyKey);
        transaction.Should().NotBeNull();
        transaction!.Status.Should().Be(TransactionStatus.Completed);
        transaction.Amount.Should().Be(250m);

        // Verify ledger entries
        var ledgerEntries = await _dbContext.LedgerEntries
            .Where(l => l.TransactionId == transaction.Id)
            .ToListAsync();
        ledgerEntries.Should().HaveCount(2);

        // Verify account balances
        var updatedFromAccount = await _dbContext.Accounts.FindAsync(fromAccountId);
        var updatedToAccount = await _dbContext.Accounts.FindAsync(toAccountId);

        updatedFromAccount!.Balance.Should().Be(750m); // 1000 - 250
        updatedToAccount!.Balance.Should().Be(750m);   // 500 + 250
    }

    [Fact]
    public async Task PostPayment_WithInsufficientFunds_ShouldFail()
    {
        // Arrange
        var fromAccountId = Guid.NewGuid();
        var toAccountId = Guid.NewGuid();
        var customerId1 = Guid.NewGuid();
        var customerId2 = Guid.NewGuid();

        var fromAccount = CreateTestAccount(fromAccountId, "ACC003", "Poor Sender", customerId1, 100m);
        var toAccount = CreateTestAccount(toAccountId, "ACC004", "Receiver", customerId2, 500m);

        await _dbContext.Accounts.AddAsync(fromAccount);
        await _dbContext.Accounts.AddAsync(toAccount);
        await _dbContext.SaveChangesAsync();

        var command = new PostPaymentCommand(
            fromAccountId: fromAccountId,
            toAccountId: toAccountId,
            amount: 250m, // More than available balance
            currency: "USD",
            paymentReference: "PAY-002",
            paymentDescription: "Payment exceeding balance",
            idempotencyKey: Guid.NewGuid().ToString()
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("balance");

        // Verify transaction was NOT created
        var transaction = await _dbContext.Transactions
            .FirstOrDefaultAsync(t => t.IdempotencyKey == command.IdempotencyKey);
        transaction.Should().BeNull();

        // Verify account balances unchanged
        var unchangedFromAccount = await _dbContext.Accounts.FindAsync(fromAccountId);
        unchangedFromAccount!.Balance.Should().Be(100m);
    }

    [Fact]
    public async Task PostPayment_WithInvalidFromAccount_ShouldFail()
    {
        // Arrange
        var fromAccountId = Guid.NewGuid();
        var toAccountId = Guid.NewGuid();
        var customerId2 = Guid.NewGuid();

        var toAccount = CreateTestAccount(toAccountId, "ACC005", "Receiver", customerId2, 500m);
        await _dbContext.Accounts.AddAsync(toAccount);
        await _dbContext.SaveChangesAsync();

        var command = new PostPaymentCommand(
            fromAccountId: fromAccountId,
            toAccountId: toAccountId,
            amount: 100m,
            currency: "USD",
            paymentReference: "PAY-003",
            paymentDescription: "Invalid sender",
            idempotencyKey: Guid.NewGuid().ToString()
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("not found");
    }

    [Fact]
    public async Task PostPayment_WithInvalidToAccount_ShouldFail()
    {
        // Arrange
        var fromAccountId = Guid.NewGuid();
        var toAccountId = Guid.NewGuid();
        var customerId1 = Guid.NewGuid();

        var fromAccount = CreateTestAccount(fromAccountId, "ACC006", "Sender", customerId1, 1000m);
        await _dbContext.Accounts.AddAsync(fromAccount);
        await _dbContext.SaveChangesAsync();

        var command = new PostPaymentCommand(
            fromAccountId: fromAccountId,
            toAccountId: toAccountId,
            amount: 100m,
            currency: "USD",
            paymentReference: "PAY-004",
            paymentDescription: "Invalid receiver",
            idempotencyKey: Guid.NewGuid().ToString()
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("not found");
    }

    [Fact]
    public async Task PostPayment_WithIdempotencyKey_ShouldPreventDuplicates()
    {
        // Arrange
        var fromAccountId = Guid.NewGuid();
        var toAccountId = Guid.NewGuid();
        var customerId1 = Guid.NewGuid();
        var customerId2 = Guid.NewGuid();
        var idempotencyKey = Guid.NewGuid().ToString();

        var fromAccount = CreateTestAccount(fromAccountId, "ACC007", "Sender", customerId1, 1000m);
        var toAccount = CreateTestAccount(toAccountId, "ACC008", "Receiver", customerId2, 500m);

        await _dbContext.Accounts.AddAsync(fromAccount);
        await _dbContext.Accounts.AddAsync(toAccount);
        await _dbContext.SaveChangesAsync();

        var command = new PostPaymentCommand(
            fromAccountId: fromAccountId,
            toAccountId: toAccountId,
            amount: 100m,
            currency: "USD",
            paymentReference: "PAY-005",
            paymentDescription: "Idempotent payment",
            idempotencyKey: idempotencyKey
        );

        // Act - First call
        var result1 = await _handler.Handle(command, CancellationToken.None);

        // Store initial balances
        var firstFromBalance = (await _dbContext.Accounts.FindAsync(fromAccountId))!.Balance;
        var firstToBalance = (await _dbContext.Accounts.FindAsync(toAccountId))!.Balance;

        // Act - Second call with same idempotency key
        var result2 = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result1.IsSuccess.Should().BeTrue();
        result2.IsSuccess.Should().BeTrue();

        // Verify only one transaction was created
        var transactions = await _dbContext.Transactions
            .Where(t => t.IdempotencyKey == idempotencyKey)
            .ToListAsync();
        transactions.Should().HaveCount(1);

        // Verify balances didn't change on second call
        var secondFromBalance = (await _dbContext.Accounts.FindAsync(fromAccountId))!.Balance;
        var secondToBalance = (await _dbContext.Accounts.FindAsync(toAccountId))!.Balance;

        secondFromBalance.Should().Be(firstFromBalance);
        secondToBalance.Should().Be(firstToBalance);
    }

    private static Account CreateTestAccount(
        Guid id,
        string accountNumber,
        string holderName,
        Guid customerId,
        decimal balance)
    {
        return new Account
        {
            Id = id,
            AccountNumber = accountNumber,
            AccountHolderName = holderName,
            CustomerId = customerId,
            Balance = balance,
            Currency = "USD",
            Status = AccountStatus.Active,
            Type = AccountType.Checking,
            OpenedAtUtc = DateTime.UtcNow.AddDays(-365),
            LastActivityDate = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}

/// <summary>
/// Test command for posting payment
/// </summary>
public sealed record PostPaymentCommand(
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount,
    string Currency,
    string PaymentReference,
    string PaymentDescription,
    string IdempotencyKey
);

/// <summary>
/// Test result for posting payment
/// </summary>
public sealed record PostPaymentResult(
    bool IsSuccess,
    string? ErrorMessage = null
);
