using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Application.Interfaces;
using Bank.Application.Services;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Bank.Tests
{
    public class AccountLifecycleServiceTests
    {
        private static Account NewAccount(Guid id, Guid userId, AccountStatus status = AccountStatus.Active)
        {
            return new Account
            {
                Id = id,
                AccountNumber = $"ACC-{id.ToString().Substring(0, 8)}",
                AccountHolderName = "Holder",
                Status = status,
                UserId = userId,
                LastActivityDate = DateTime.UtcNow.AddDays(-120)
            };
        }

        private static AccountLifecycleService CreateService(
            Mock<IUnitOfWork> uow,
            out Mock<IRepository<Account>> accountRepo,
            out Mock<IRepository<AccountStatusHistory>> historyRepo,
            out Mock<IRepository<AccountHold>> holdRepo,
            out Mock<IRepository<AccountFee>> feeRepo,
            out Mock<IFeeCalculationService> feeCalc,
            out Mock<IInterestCalculationService> interestCalc,
            out Mock<IAuditLogService> audit,
            out Mock<ILogger<AccountLifecycleService>> logger)
        {
            accountRepo = new Mock<IRepository<Account>>();
            historyRepo = new Mock<IRepository<AccountStatusHistory>>();
            holdRepo = new Mock<IRepository<AccountHold>>();
            feeRepo = new Mock<IRepository<AccountFee>>();
            feeCalc = new Mock<IFeeCalculationService>();
            interestCalc = new Mock<IInterestCalculationService>();
            audit = new Mock<IAuditLogService>();
            logger = new Mock<ILogger<AccountLifecycleService>>();

            uow.Setup(u => u.Repository<Account>()).Returns(accountRepo.Object);
            uow.Setup(u => u.Repository<AccountStatusHistory>()).Returns(historyRepo.Object);
            uow.Setup(u => u.Repository<AccountHold>()).Returns(holdRepo.Object);
            uow.Setup(u => u.Repository<AccountFee>()).Returns(feeRepo.Object);
            uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

            return new AccountLifecycleService(
                uow.Object,
                feeCalc.Object,
                interestCalc.Object,
                audit.Object,
                logger.Object);
        }

        [Fact]
        public async Task CloseAccount_Should_Succeed_When_Active_No_Holds()
        {
            // Arrange
            var uow = new Mock<IUnitOfWork>();
            var service = CreateService(uow,
                out var accountRepo,
                out var historyRepo,
                out _, out _, out var feeCalc, out _, out var audit, out _);

            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var acc = NewAccount(id, userId, AccountStatus.Active);
            acc.HasHolds = false;

            accountRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(acc);
            historyRepo.Setup(r => r.AddAsync(It.IsAny<AccountStatusHistory>())).Returns(Task.CompletedTask);
            accountRepo.Setup(r => r.Update(It.IsAny<Account>()));
            audit.Setup(a => a.LogUserActionAsync(userId, "ACCOUNT_CLOSED", "Account", id.ToString(), null, It.IsAny<string>())).Returns(Task.CompletedTask);
            feeCalc.Setup(f => f.CalculateMaintenanceFeeAsync(acc, It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(0m);

            // Act
            var ok = await service.CloseAccountAsync(id, "reason", userId);

            // Assert
            Assert.True(ok);
            Assert.Equal(AccountStatus.Closed, acc.Status);
            historyRepo.Verify(r => r.AddAsync(It.IsAny<AccountStatusHistory>()), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task ReopenAccount_Should_Require_Closed_Status()
        {
            // Arrange
            var uow = new Mock<IUnitOfWork>();
            var service = CreateService(uow,
                out var accountRepo,
                out var historyRepo,
                out _, out _, out _, out _, out var audit, out _);

            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var acc = NewAccount(id, userId, AccountStatus.Closed);

            accountRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(acc);
            historyRepo.Setup(r => r.AddAsync(It.IsAny<AccountStatusHistory>())).Returns(Task.CompletedTask);
            accountRepo.Setup(r => r.Update(It.IsAny<Account>()));
            audit.Setup(a => a.LogUserActionAsync(userId, "ACCOUNT_REOPENED", "Account", id.ToString(), null, It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            var ok = await service.ReopenAccountAsync(id, userId);

            // Assert
            Assert.True(ok);
            Assert.Equal(AccountStatus.Active, acc.Status);
            historyRepo.Verify(r => r.AddAsync(It.IsAny<AccountStatusHistory>()), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task MarkAccountDormant_Should_Set_Dormant_Status()
        {
            // Arrange
            var uow = new Mock<IUnitOfWork>();
            var service = CreateService(uow,
                out var accountRepo,
                out var historyRepo,
                out _, out _, out _, out _, out var audit, out _);

            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var acc = NewAccount(id, userId, AccountStatus.Active);

            accountRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(acc);
            historyRepo.Setup(r => r.AddAsync(It.IsAny<AccountStatusHistory>())).Returns(Task.CompletedTask);
            accountRepo.Setup(r => r.Update(It.IsAny<Account>()));
            audit.Setup(a => a.LogSystemEventAsync("ACCOUNT_MARKED_DORMANT", "Account", id.ToString(), It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            var ok = await service.MarkAccountDormantAsync(id);

            // Assert
            Assert.True(ok);
            Assert.Equal(AccountStatus.Dormant, acc.Status);
            historyRepo.Verify(r => r.AddAsync(It.IsAny<AccountStatusHistory>()), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task ApplyAccountFees_Should_Create_Fee_And_Debit_When_TotalFees_Gt_Zero()
        {
            // Arrange
            var uow = new Mock<IUnitOfWork>();
            var service = CreateService(uow,
                out var accountRepo,
                out _,
                out _, out var feeRepo,
                out var feeCalc, out _, out var audit, out _);

            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var acc = NewAccount(id, userId, AccountStatus.Active);
            acc.MonthlyMaintenanceFee = 10m;
            acc.Balance = 100m;

            accountRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(acc);
            feeRepo.Setup(r => r.AddAsync(It.IsAny<AccountFee>())).Returns(Task.CompletedTask);
            accountRepo.Setup(r => r.Update(It.IsAny<Account>()));
            audit.Setup(a => a.LogUserActionAsync(userId, "FEES_APPLIED", "Account", id.ToString(), null, It.IsAny<string>())).Returns(Task.CompletedTask);

            // Make CalculateAccountFeesAsync return > 0 by stubbing feeCalc
            feeCalc.Setup(f => f.CalculateMaintenanceFeeAsync(acc, It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(10m);

            // Act
            var ok = await service.ApplyAccountFeesAsync(id, userId);

            // Assert
            Assert.True(ok);
            Assert.Equal(90m, acc.Balance);
            feeRepo.Verify(r => r.AddAsync(It.IsAny<AccountFee>()), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task AddRestriction_Should_Create_New_Active_Restriction_When_None_Exists()
        {
            // Arrange
            var uow = new Mock<IUnitOfWork>();
            var service = CreateService(uow,
                out var accountRepo,
                out _,
                out _, out _,
                out _, out _, out var audit, out _);

            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var acc = NewAccount(id, userId);

            accountRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(acc);

            var restrictionRepo = new Mock<IRepository<AccountRestriction>>();
            restrictionRepo.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<AccountRestriction, bool>>>() ))
                .ReturnsAsync((AccountRestriction?)null);
            restrictionRepo.Setup(r => r.AddAsync(It.IsAny<AccountRestriction>())).Returns(Task.CompletedTask);
            uow.Setup(u => u.Repository<AccountRestriction>()).Returns(restrictionRepo.Object);

            accountRepo.Setup(r => r.Update(It.IsAny<Account>()));
            audit.Setup(a => a.LogUserActionAsync(userId, "RESTRICTION_ADDED", "Account", id.ToString(), null, It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            var ok = await service.AddRestrictionAsync(id, AccountRestrictionType.NoDebits, "reason", userId);

            // Assert
            Assert.True(ok);
            Assert.True(acc.HasRestrictions);
            restrictionRepo.Verify(r => r.AddAsync(It.IsAny<AccountRestriction>()), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }
    }
}
