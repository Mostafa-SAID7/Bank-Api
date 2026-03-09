using System;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Xunit;

namespace Bank.Domain.UnitTests
{
    public class AccountRestrictionDomainTests
    {
        [Fact]
        public void IsActive_Should_Be_True_When_No_RemovedDate_And_Future_Expiry()
        {
            // Arrange
            var r = new AccountRestriction
            {
                Id = Guid.NewGuid(),
                AccountId = Guid.NewGuid(),
                Type = AccountRestrictionType.NoDebits,
                AppliedDate = DateTime.UtcNow.AddDays(-1),
                ExpiryDate = DateTime.UtcNow.AddDays(1)
            };

            // Assert
            Assert.True(r.IsActive);
        }

        [Fact]
        public void IsActive_Should_Be_False_When_RemovedDate_Set()
        {
            // Arrange
            var r = new AccountRestriction
            {
                Id = Guid.NewGuid(),
                AccountId = Guid.NewGuid(),
                Type = AccountRestrictionType.NoDebits,
                AppliedDate = DateTime.UtcNow.AddDays(-10),
                RemovedDate = DateTime.UtcNow.AddDays(-1)
            };

            // Assert
            Assert.False(r.IsActive);
        }

        [Fact]
        public void IsActive_Should_Be_False_When_Expired()
        {
            // Arrange
            var r = new AccountRestriction
            {
                Id = Guid.NewGuid(),
                AccountId = Guid.NewGuid(),
                Type = AccountRestrictionType.NoCredits,
                AppliedDate = DateTime.UtcNow.AddDays(-10),
                ExpiryDate = DateTime.UtcNow.AddDays(-1)
            };

            // Assert
            Assert.False(r.IsActive);
        }

        [Fact]
        public void Remove_Should_Set_Fields_And_Append_Notes()
        {
            // Arrange
            var user = Guid.NewGuid();
            var r = new AccountRestriction
            {
                Id = Guid.NewGuid(),
                AccountId = Guid.NewGuid(),
                Type = AccountRestrictionType.NoTransfers,
                AppliedDate = DateTime.UtcNow.AddDays(-1),
                Notes = "Initial"
            };

            // Act
            r.Remove(user, "Removed for reason");

            // Assert
            Assert.False(r.IsActive);
            Assert.Equal(user, r.RemovedByUserId);
            Assert.Contains("Removed for reason", r.Notes);
        }
    }
}
