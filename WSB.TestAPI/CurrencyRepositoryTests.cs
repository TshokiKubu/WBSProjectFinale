using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using WSB.API.Data;
using WSB.API.Models;
using WSB.API.Repository;
using Xunit;

namespace WSB.TestAPI
{
    public class CurrencyRepositoryTests
    {
        [Fact]
        public async Task GetAllCurrencies_Should_Return_Currencies()
        {
            // Arrange
            var mockContext = new Mock<ApplicationDbContext>();
            var currencyRepository = new CurrencyRepository(mockContext.Object);

            var currencies = new List<Currency>
        {
            new Currency { CurrencyId = 1, Code = "USD", Name = "US Dollar" },
            new Currency { CurrencyId = 2, Code = "EUR", Name = "Euro" }
            // Add more currencies as needed
        };

            var mockDbSet = new Mock<DbSet<Currency>>();
            mockDbSet.As<IQueryable<Currency>>().Setup(m => m.Provider).Returns(currencies.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Currency>>().Setup(m => m.Expression).Returns(currencies.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Currency>>().Setup(m => m.ElementType).Returns(currencies.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<Currency>>().Setup(m => m.GetEnumerator()).Returns(currencies.AsQueryable().GetEnumerator());

            mockContext.Setup(x => x.Currencies).Returns(mockDbSet.Object);

            // Act
            var result = await currencyRepository.GetAllCurrencies();

            // Assert
            Assert.Equal(currencies, result);
        }

        [Fact]
        public async Task AddCurrency_Should_Add_To_Context()
        {
            // Arrange
            var mockContext = new Mock<ApplicationDbContext>();
            var currencyRepository = new CurrencyRepository(mockContext.Object);

            //var newCurrency = new Currency { CurrencyId = 3, Code = "GBP", Name = "British Pound" };
            var newCurrency = new Currency { CurrencyId = 3, Code = "ZAR", Name = "South African Rand" };

            // Act
            var result = await currencyRepository.AddCurrency(newCurrency);

            // Assert
            Assert.Equal(1, result); // Assuming 1 row affected
            mockContext.Verify(x => x.Currencies.Add(It.IsAny<Currency>()), Times.Once);           
            mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        }
    }
}