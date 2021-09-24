using CsvApp.Business.Interfaces;
using CsvApp.Business.Services;
using EnergyDataLayer.Context;
using Moq;
using NUnit.Framework;

namespace CsvApp.Business.Tests
{
    public class EnergyServiceTests
    {
        private Mock<EfEnergyDbService> _energyService;
        private Mock<EnergyDbContext> _context;

        [SetUp]
        public void Setup()
        {
            _context = new Mock<EnergyDbContext>();
            _energyService = new Mock<EfEnergyDbService>(_context.Object);
            // Add accounts
            _context.Object.Accounts.Add(new Account {AccountId = 1, FirstName = "Bryan", LastName = "Cranston"});
            _context.Object.Accounts.Add(new Account {AccountId = 2, FirstName = "Liam", LastName = "Neeson"});
            _context.Object.Accounts.Add(new Account {AccountId = 3, FirstName = "Jon", LastName = "Snow"});
            _context.Object.Accounts.Add(new Account {AccountId = 4, FirstName = "Bruce", LastName = "Lee"});
        }

        [Test]
        public void TestGetAccountReturnsFourAccounts()
        {
            Assert.Pass();

            // Arrange

            // Act

            // Assert
        }
    }
}