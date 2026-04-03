using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Xunit;

namespace Bank.Tests;

/// <summary>
/// Unit tests for card management functionality
/// </summary>
public class CardManagementTests
{
    [Fact]
    public void Card_ShouldBeCreatedWithCorrectDefaults()
    {
        // Arrange & Act
        var card = new Card
        {
            CardNumber = "4000123456789012",
            CustomerId = Guid.NewGuid(),
            AccountId = Guid.NewGuid(),
            Type = CardType.Debit,
            ExpiryDate = DateTime.UtcNow.AddYears(3)
        };

        // Assert
        Assert.Equal(CardStatus.Inactive, card.Status);
        Assert.Equal(5000m, card.DailyLimit);
        Assert.Equal(50000m, card.MonthlyLimit);
        Assert.Equal(2000m, card.AtmDailyLimit);
        Assert.True(card.ContactlessEnabled);
        Assert.True(card.OnlineTransactionsEnabled);
        Assert.False(card.InternationalTransactionsEnabled);
        Assert.Equal(0, card.FailedPinAttempts);
    }

    [Fact]
    public void Card_IsActive_ShouldReturnTrueForActiveCard()
    {
        // Arrange
        var card = new Card
        {
            Status = CardStatus.Active,
            ExpiryDate = DateTime.UtcNow.AddYears(1),
            FailedPinAttempts = 0
        };

        // Act & Assert
        Assert.True(card.IsActive());
    }

    [Fact]
    public void Card_IsActive_ShouldReturnFalseForExpiredCard()
    {
        // Arrange
        var card = new Card
        {
            Status = CardStatus.Active,
            ExpiryDate = DateTime.UtcNow.AddDays(-1),
            FailedPinAttempts = 0
        };

        // Act & Assert
        Assert.False(card.IsActive());
    }

    [Fact]
    public void Card_IsBlocked_ShouldReturnTrueForBlockedCard()
    {
        // Arrange
        var card = new Card
        {
            Status = CardStatus.Blocked
        };

        // Act & Assert
        Assert.True(card.IsBlocked());
    }

    [Fact]
    public void Card_Activate_ShouldChangeStatusToActive()
    {
        // Arrange
        var card = new Card
        {
            Status = CardStatus.Inactive
        };

        // Act
        card.Activate(CardActivationChannel.Online);

        // Assert
        Assert.Equal(CardStatus.Active, card.Status);
        Assert.Equal(CardActivationChannel.Online, card.ActivationChannel);
        Assert.NotNull(card.ActivationDate);
    }

    [Fact]
    public void Card_Block_ShouldChangeStatusToBlocked()
    {
        // Arrange
        var card = new Card
        {
            Status = CardStatus.Active
        };

        // Act
        card.Block(CardBlockReason.CustomerRequest);

        // Assert
        Assert.Equal(CardStatus.Blocked, card.Status);
        Assert.Equal(CardBlockReason.CustomerRequest, card.LastBlockReason);
        Assert.NotNull(card.LastBlockedDate);
    }

    [Fact]
    public void Card_GenerateMaskedCardNumber_ShouldMaskCorrectly()
    {
        // Arrange
        var cardNumber = "4000123456789012";

        // Act
        var maskedNumber = Card.GenerateMaskedCardNumber(cardNumber);

        // Assert
        Assert.Equal("****-****-****-9012", maskedNumber);
    }

    [Fact]
    public void CardTransaction_Authorize_ShouldSetStatusToAuthorized()
    {
        // Arrange
        var transaction = new CardTransaction
        {
            Status = CardTransactionStatus.Pending
        };

        // Act
        transaction.Authorize("AUTH123");

        // Assert
        Assert.Equal(CardTransactionStatus.Authorized, transaction.Status);
        Assert.Equal("AUTH123", transaction.AuthorizationCode);
        Assert.NotNull(transaction.AuthorizationDate);
    }

    [Fact]
    public void CardTransaction_Settle_ShouldSetStatusToSettled()
    {
        // Arrange
        var transaction = new CardTransaction
        {
            Status = CardTransactionStatus.Authorized
        };

        // Act
        transaction.Settle();

        // Assert
        Assert.Equal(CardTransactionStatus.Settled, transaction.Status);
        Assert.NotNull(transaction.SettlementDate);
    }

    [Fact]
    public void CardTransaction_IsSuccessful_ShouldReturnTrueForAuthorizedOrSettled()
    {
        // Arrange
        var authorizedTransaction = new CardTransaction { Status = CardTransactionStatus.Authorized };
        var settledTransaction = new CardTransaction { Status = CardTransactionStatus.Settled };
        var pendingTransaction = new CardTransaction { Status = CardTransactionStatus.Pending };

        // Act & Assert
        Assert.True(authorizedTransaction.IsSuccessful());
        Assert.True(settledTransaction.IsSuccessful());
        Assert.False(pendingTransaction.IsSuccessful());
    }

    [Fact]
    public void CardTransaction_GetTotalAmount_ShouldIncludeFees()
    {
        // Arrange
        var transaction = new CardTransaction
        {
            Amount = 100m,
            Fee = 2.50m
        };

        // Act
        var totalAmount = transaction.GetTotalAmount();

        // Assert
        Assert.Equal(102.50m, totalAmount);
    }
}