using System;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Xunit;

namespace Bank.Domain.UnitTests
{
    public class AccountDomainTests
    {
        private static Account NewAccount(decimal balance = 0m, AccountStatus status = AccountStatus.Active)
        {
            return new Account
            {
                Id = Guid.NewGuid(),
                AccountNumber = "ACC-123",
                AccountHolderName = "User",
                Balance = balance,
                Status = status,
                UserId = Guid.NewGuid(),
                LastActivityDate = DateTime.UtcNow.AddDays(-10),
                DormancyPeriodDays = 5
            };
        }

        [Fact]
        public void IsDormant_Should_Return_True_When_LastActivity_Exceeds_Period()
        {
            // Arrange
            var acc = NewAccount(status: AccountStatus.Active);

            // Act
            var dormant = acc.IsDormant();

            // Assert
            Assert.True(dormant);
        }

        [Fact]
        public void IsActive_Should_Return_False_When_HasHolds()
        {
            // Arrange
            var acc = NewAccount();
            acc.HasHolds = true;

            // Act
            var active = acc.IsActive();

            // Assert
            Assert.False(active);
        }

        [Fact]
        public void CanDebit_Should_Respect_Balance_And_Restrictions()
        {
            // Arrange
            var acc = NewAccount(balance: 100m);
            // Active restriction by leaving RemovedDate null and ExpiryDate in the future
            acc.Restrictions.Add(new AccountRestriction { Type = AccountRestrictionType.NoDebits, ExpiryDate = DateTime.UtcNow.AddDays(1) });

            // Act
            var canDebit100 = acc.CanDebit(100m);
            var canDebit50 = acc.CanDebit(50m);

            // Assert
            Assert.False(canDebit100);
            Assert.False(canDebit50);
        }

        [Fact]
        public void UpdateActivity_Should_Reactivate_Dormant()
        {
            // Arrange
            var acc = NewAccount();
            acc.Status = AccountStatus.Dormant;
            acc.DormancyDate = DateTime.UtcNow.AddDays(-1);

            // Act
            acc.UpdateActivity();

            // Assert
            Assert.Equal(AccountStatus.Active, acc.Status);
            Assert.Null(acc.DormancyDate);
            Assert.True((DateTime.UtcNow - acc.LastActivityDate).TotalSeconds < 5);
        }

        [Fact]
        public void RequiresMultipleSignaturesForAmount_Should_Work_For_Joint_Accounts()
        {
            // Arrange
            var acc = NewAccount();
            acc.IsJointAccount = true;
            acc.RequiresMultipleSignatures = true;
            acc.MultipleSignatureThreshold = 1000m;

            // Act & Assert
            Assert.False(acc.RequiresMultipleSignaturesForAmount(999m));
            Assert.True(acc.RequiresMultipleSignaturesForAmount(1000m));
            Assert.True(acc.RequiresMultipleSignaturesForAmount(5000m));
        }
    }
}
