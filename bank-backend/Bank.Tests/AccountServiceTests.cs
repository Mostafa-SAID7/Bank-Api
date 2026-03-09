using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Application.Services;
using Bank.Domain.Entities;
using Bank.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Bank.Tests
{
    public class AccountServiceTests
    {
        private static Account NewAccount(Guid id, Guid userId, decimal balance = 0m)
        {
            return new Account
            {
                Id = id,
                AccountNumber = $"ACC-{id.ToString().Substring(0, 8)}",
                AccountHolderName = "Holder",
                Balance = balance,
                UserId = userId
            };
        }

        [Fact]
        public async Task GetAccountById_Should_Return_Account_With_User_Included()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var account = NewAccount(id, userId);
            account.User = new User { Id = userId, Email = "user@test.com" };

            var repo = new Mock<IRepository<Account>>();
            // Emulate EF Query + Include by returning an IQueryable with this single account
            var queryable = new List<Account> { account }.AsQueryable();
            repo.Setup(r => r.Query()).Returns(queryable);

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.Repository<Account>()).Returns(repo.Object);

            var svc = new AccountService(uow.Object);

            // Act
            var result = await svc.GetAccountByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result!.Id);
            Assert.NotNull(result.User);
        }

        [Fact]
        public async Task CreateAccount_Should_Add_And_Save()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var repo = new Mock<IRepository<Account>>();
            Account? saved = null;
            repo.Setup(r => r.AddAsync(It.IsAny<Account>()))
                .Callback<Account>(a => saved = a)
                .Returns(Task.CompletedTask);

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.Repository<Account>()).Returns(repo.Object);
            uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

            var svc = new AccountService(uow.Object);

            // Act
            var result = await svc.CreateAccountAsync(userId, "Holder");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.StartsWith("BNK-", result.AccountNumber);
            repo.Verify(r => r.AddAsync(It.IsAny<Account>()), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task UpdateBalance_Should_Return_False_When_Account_Not_Found()
        {
            // Arrange
            var id = Guid.NewGuid();
            var repo = new Mock<IRepository<Account>>();
            repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Account?)null);

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.Repository<Account>()).Returns(repo.Object);

            var svc = new AccountService(uow.Object);

            // Act
            var ok = await svc.UpdateBalanceAsync(id, 100m);

            // Assert
            Assert.False(ok);
        }

        [Fact]
        public async Task UpdateBalance_Should_Update_And_Save_When_Account_Exists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var account = NewAccount(id, userId, 50m);

            var repo = new Mock<IRepository<Account>>();
            repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(account);
            repo.Setup(r => r.Update(It.IsAny<Account>()));

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.Repository<Account>()).Returns(repo.Object);
            uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

            var svc = new AccountService(uow.Object);

            // Act
            var ok = await svc.UpdateBalanceAsync(id, 25m);

            // Assert
            Assert.True(ok);
            Assert.Equal(75m, account.Balance);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task CanUserAccessAccount_Should_Check_CanUserAccess()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var acc = NewAccount(id, userId);
            acc.JointHolders = new List<JointAccountHolder>();

            var repo = new Mock<IRepository<Account>>();
            var queryable = new List<Account> { acc }.AsQueryable();
            repo.Setup(r => r.Query()).Returns(queryable);

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.Repository<Account>()).Returns(repo.Object);

            var svc = new AccountService(uow.Object);

            // Act
            var ok = await svc.CanUserAccessAccountAsync(id, userId);

            // Assert
            Assert.True(ok);
        }
    }
}
