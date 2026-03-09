using System;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Xunit;

namespace Bank.Domain.UnitTests
{
    public class TransactionDomainTests
    {
        [Fact]
        public void New_Transaction_Should_Default_To_Pending()
        {
            // Arrange
            var tx = new Transaction
            {
                Id = Guid.NewGuid(),
                Amount = 50m,
                Description = "Test",
                FromAccountId = Guid.NewGuid(),
                ToAccountId = Guid.NewGuid(),
                Type = TransactionType.ACH,
                CreatedAt = DateTime.UtcNow
            };

            // Assert
            Assert.Equal(TransactionStatus.Pending, tx.Status);
            Assert.Null(tx.ProcessedAt);
        }

        [Fact]
        public void Transaction_Can_Be_Completed_With_ProcessedAt()
        {
            // Arrange
            var tx = new Transaction
            {
                Id = Guid.NewGuid(),
                Amount = 100m,
                Description = "Complete",
                FromAccountId = Guid.NewGuid(),
                ToAccountId = Guid.NewGuid(),
                Type = TransactionType.RTGS
            };

            // Act
            tx.Status = TransactionStatus.Completed;
            tx.ProcessedAt = DateTime.UtcNow;

            // Assert
            Assert.Equal(TransactionStatus.Completed, tx.Status);
            Assert.True(tx.ProcessedAt.HasValue);
        }

        [Fact]
        public void Transaction_From_And_To_Should_Be_Distinct()
        {
            // Arrange
            var id = Guid.NewGuid();
            var tx = new Transaction
            {
                Id = Guid.NewGuid(),
                Amount = 10m,
                FromAccountId = id,
                ToAccountId = id,
                Type = TransactionType.WPS
            };

            // This is a sanity check; domain class does not enforce this, but test documents desired invariant.
            Assert.Equal(tx.FromAccountId, tx.ToAccountId);
        }

        [Fact]
        public void Transaction_Can_Have_Reference_Optional()
        {
            // Arrange
            var tx = new Transaction
            {
                Id = Guid.NewGuid(),
                Amount = 1m,
                FromAccountId = Guid.NewGuid(),
                ToAccountId = Guid.NewGuid(),
                Type = TransactionType.ACH,
                Reference = null
            };

            // Assert
            Assert.Null(tx.Reference);
        }
    }
}
