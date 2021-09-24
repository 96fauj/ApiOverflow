using System.Collections.Generic;
using System.Linq;
using CsvApp.Business.Interfaces;
using CsvApp.Business.Services;
using EnergyDataLayer.Context;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace CsvApp.Business.Tests
{
    public class EnergyServiceTests
    {
        private EfEnergyDbService _energyService;
        private Mock<EnergyDbContext> _dbContextMock;
        private List<Account> _accounts;
        //private DbContextOptions<EnergyDbContext> _options;
 

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptions<EnergyDbContext>();
            _dbContextMock = new Mock<EnergyDbContext>(options);
            _energyService = new EfEnergyDbService(_dbContextMock.Object);

            // Accounts list
            _accounts = new List<Account>();
            _accounts.Add(new Account { AccountId = 3, FirstName = "Jon", LastName = "Snow" });
            _accounts.Add(new Account { AccountId = 4, FirstName = "Bruce", LastName = "Lee" });
            _accounts.Add(new Account { AccountId = 1, FirstName = "Bryan", LastName = "Cranston" });
            _accounts.Add(new Account { AccountId = 2, FirstName = "Liam", LastName = "Neeson" });
            
            _dbContextMock.Setup(x => x.Set<Account>())
                .Returns(GetDbSetMock(_accounts).Object);
        }

        [Test]
        public void GetAllAccounts_ShouldReturn4SeedAccounts()
        {
            //Assert.Pass();

            // Arrange

            // Act
            var accounts = _energyService.GetAllAccounts();

            // Assert
            Assert.AreEqual(4, accounts.Count());
        }

        private static Mock<DbSet<T>> GetDbSetMock<T>(IEnumerable<T> items = null) where T : class
        {
            if (items == null)
            {
                items = new T[0];
            }

            var dbSetMock = new Mock<DbSet<T>>();
            var q = dbSetMock.As<IQueryable<T>>();

            q.Setup(x => x.GetEnumerator()).Returns(items.GetEnumerator);

            return dbSetMock;
        }
    }
}