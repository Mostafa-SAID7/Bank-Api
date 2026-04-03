using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Application.DTOs.Payment.Biller;
using Bank.Application.Services;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Bank.Tests
{
    public class BillerServiceTests
    {
        private static Biller NewBiller(Guid id, string name, BillerCategory category, bool isActive = true)
        {
            return new Biller
            {
                Id = id,
                Name = name,
                Category = category,
                AccountNumber = "ACC-123456",
                RoutingNumber = "ROUTE-789",
                Address = "123 Main St",
                IsActive = isActive,
                SupportedPaymentMethods = "[\"ACH\", \"Wire\"]",
                MinAmount = 10m,
                MaxAmount = 10000m,
                ProcessingDays = 1,
                CreatedAt = DateTime.UtcNow
            };
        }

        [Fact]
        public async Task GetAvailableBillersAsync_Should_Return_All_Active_Billers()
        {
            // Arrange
            var billerId1 = Guid.NewGuid();
            var billerId2 = Guid.NewGuid();
            var biller1 = NewBiller(billerId1, "Electric Company", BillerCategory.Utilities, true);
            var biller2 = NewBiller(billerId2, "Water Company", BillerCategory.Utilities, true);
            var inactiveBiller = NewBiller(Guid.NewGuid(), "Inactive Biller", BillerCategory.Other, false);

            var billerRepository = new Mock<IBillerRepository>();
            billerRepository
                .Setup(r => r.GetActiveBillersAsync())
                .ReturnsAsync(new List<Biller> { biller1, biller2 });

            var logger = new Mock<ILogger<BillerService>>();
            var service = new BillerService(billerRepository.Object, logger.Object);

            // Act
            var result = await service.GetAvailableBillersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, b => Assert.True(b.IsActive));
            billerRepository.Verify(r => r.GetActiveBillersAsync(), Times.Once);
        }

        [Fact]
        public async Task GetBillersByCategoryAsync_Should_Return_Billers_For_Specified_Category()
        {
            // Arrange
            var billerId1 = Guid.NewGuid();
            var billerId2 = Guid.NewGuid();
            var biller1 = NewBiller(billerId1, "Electric Company", BillerCategory.Utilities);
            var biller2 = NewBiller(billerId2, "Gas Company", BillerCategory.Utilities);

            var billerRepository = new Mock<IBillerRepository>();
            billerRepository
                .Setup(r => r.GetBillersByCategoryAsync(BillerCategory.Utilities))
                .ReturnsAsync(new List<Biller> { biller1, biller2 });

            var logger = new Mock<ILogger<BillerService>>();
            var service = new BillerService(billerRepository.Object, logger.Object);

            // Act
            var result = await service.GetBillersByCategoryAsync(BillerCategory.Utilities);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, b => Assert.Equal(BillerCategory.Utilities, b.Category));
            billerRepository.Verify(r => r.GetBillersByCategoryAsync(BillerCategory.Utilities), Times.Once);
        }

        [Fact]
        public async Task SearchBillersAsync_Should_Return_Billers_Matching_Search_Term()
        {
            // Arrange
            var billerId = Guid.NewGuid();
            var biller = NewBiller(billerId, "Electric Company", BillerCategory.Utilities);
            var request = new BillerSearchRequest("Electric", null, true);

            var billerRepository = new Mock<IBillerRepository>();
            billerRepository
                .Setup(r => r.SearchBillersByNameAsync("Electric"))
                .ReturnsAsync(new List<Biller> { biller });

            var logger = new Mock<ILogger<BillerService>>();
            var service = new BillerService(billerRepository.Object, logger.Object);

            // Act
            var result = await service.SearchBillersAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Electric Company", result[0].Name);
            billerRepository.Verify(r => r.SearchBillersByNameAsync("Electric"), Times.Once);
        }

        [Fact]
        public async Task SearchBillersAsync_Should_Filter_By_Category_When_No_Search_Term()
        {
            // Arrange
            var billerId = Guid.NewGuid();
            var biller = NewBiller(billerId, "Internet Provider", BillerCategory.Telecom);
            var request = new BillerSearchRequest(null, BillerCategory.Telecom, true);

            var billerRepository = new Mock<IBillerRepository>();
            billerRepository
                .Setup(r => r.GetBillersByCategoryAsync(BillerCategory.Telecom))
                .ReturnsAsync(new List<Biller> { biller });

            var logger = new Mock<ILogger<BillerService>>();
            var service = new BillerService(billerRepository.Object, logger.Object);

            // Act
            var result = await service.SearchBillersAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(BillerCategory.Telecom, result[0].Category);
            billerRepository.Verify(r => r.GetBillersByCategoryAsync(BillerCategory.Telecom), Times.Once);
        }

        [Fact]
        public async Task SearchBillersAsync_Should_Return_All_Active_Billers_When_No_Criteria()
        {
            // Arrange
            var billerId1 = Guid.NewGuid();
            var billerId2 = Guid.NewGuid();
            var biller1 = NewBiller(billerId1, "Biller 1", BillerCategory.Utilities, true);
            var biller2 = NewBiller(billerId2, "Biller 2", BillerCategory.Telecom, true);
            var request = new BillerSearchRequest(null, null, true);

            var billerRepository = new Mock<IBillerRepository>();
            billerRepository
                .Setup(r => r.GetActiveBillersAsync())
                .ReturnsAsync(new List<Biller> { biller1, biller2 });

            var logger = new Mock<ILogger<BillerService>>();
            var service = new BillerService(billerRepository.Object, logger.Object);

            // Act
            var result = await service.SearchBillersAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            billerRepository.Verify(r => r.GetActiveBillersAsync(), Times.Once);
        }

        [Fact]
        public async Task SearchBillersAsync_Should_Filter_Inactive_Billers_When_ActiveOnly_True()
        {
            // Arrange
            var billerId1 = Guid.NewGuid();
            var billerId2 = Guid.NewGuid();
            var activeBiller = NewBiller(billerId1, "Active Biller", BillerCategory.Utilities, true);
            var inactiveBiller = NewBiller(billerId2, "Inactive Biller", BillerCategory.Utilities, false);
            var request = new BillerSearchRequest(null, BillerCategory.Utilities, true);

            var billerRepository = new Mock<IBillerRepository>();
            billerRepository
                .Setup(r => r.GetBillersByCategoryAsync(BillerCategory.Utilities))
                .ReturnsAsync(new List<Biller> { activeBiller, inactiveBiller });

            var logger = new Mock<ILogger<BillerService>>();
            var service = new BillerService(billerRepository.Object, logger.Object);

            // Act
            var result = await service.SearchBillersAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.True(result[0].IsActive);
        }

        [Fact]
        public async Task GetBillerByIdAsync_Should_Return_Biller_When_Found()
        {
            // Arrange
            var billerId = Guid.NewGuid();
            var biller = NewBiller(billerId, "Electric Company", BillerCategory.Utilities);

            var billerRepository = new Mock<IBillerRepository>();
            billerRepository
                .Setup(r => r.GetByIdAsync(billerId))
                .ReturnsAsync(biller);

            var logger = new Mock<ILogger<BillerService>>();
            var service = new BillerService(billerRepository.Object, logger.Object);

            // Act
            var result = await service.GetBillerByIdAsync(billerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(billerId, result.Id);
            Assert.Equal("Electric Company", result.Name);
            Assert.Equal(BillerCategory.Utilities, result.Category);
            billerRepository.Verify(r => r.GetByIdAsync(billerId), Times.Once);
        }

        [Fact]
        public async Task GetBillerByIdAsync_Should_Return_Null_When_Not_Found()
        {
            // Arrange
            var billerId = Guid.NewGuid();

            var billerRepository = new Mock<IBillerRepository>();
            billerRepository
                .Setup(r => r.GetByIdAsync(billerId))
                .ReturnsAsync((Biller?)null);

            var logger = new Mock<ILogger<BillerService>>();
            var service = new BillerService(billerRepository.Object, logger.Object);

            // Act
            var result = await service.GetBillerByIdAsync(billerId);

            // Assert
            Assert.Null(result);
            billerRepository.Verify(r => r.GetByIdAsync(billerId), Times.Once);
        }

        [Fact]
        public async Task GetBillerByIdAsync_Should_Map_Supported_Payment_Methods_Correctly()
        {
            // Arrange
            var billerId = Guid.NewGuid();
            var biller = NewBiller(billerId, "Electric Company", BillerCategory.Utilities);
            biller.SupportedPaymentMethods = "[\"ACH\", \"Wire\", \"Check\"]";

            var billerRepository = new Mock<IBillerRepository>();
            billerRepository
                .Setup(r => r.GetByIdAsync(billerId))
                .ReturnsAsync(biller);

            var logger = new Mock<ILogger<BillerService>>();
            var service = new BillerService(billerRepository.Object, logger.Object);

            // Act
            var result = await service.GetBillerByIdAsync(billerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.SupportedPaymentMethods.Length);
            Assert.Contains("ACH", result.SupportedPaymentMethods);
            Assert.Contains("Wire", result.SupportedPaymentMethods);
            Assert.Contains("Check", result.SupportedPaymentMethods);
        }
    }
}
