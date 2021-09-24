using System;
using System.Collections.Generic;
using System.Linq;
using CsvApp.Business.Parsers;
using CsvApp.Business.Services;
using EnergyDataLayer.Context;
using EnergyDataLayer.Helpers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace CsvApp.Business.Tests
{
    public class DataPersistenceTests
    {
        private EfEnergyDbService _energyService;
        private EnergyDbContext _context;
        private Repository<MeterReading> _meterReadingRepository;
        private Repository<Account> _accountRepository;
        private DateTime _baseline = new DateTime(2021, 8, 5, 10, 15, 5);

        [SetUp]
        public void Setup()
        {
            var dbContextOptions = new DbContextOptionsBuilder<EnergyDbContext>()
                .UseInMemoryDatabase("EnergyDb");
            _context = new EnergyDbContext(dbContextOptions.Options);
            _context.Database.EnsureCreated();

            _meterReadingRepository = new Repository<MeterReading>(_context);
            _accountRepository = new Repository<Account>(_context);

            _energyService = new EfEnergyDbService(_context);

            _accountRepository.Add(new Account { AccountId = 3, FirstName = "Jon", LastName = "Snow" });
            _accountRepository.Add(new Account { AccountId = 4, FirstName = "Bruce", LastName = "Lee" });
            _accountRepository.Add(new Account { AccountId = 1, FirstName = "Bryan", LastName = "Cranston" });
            _accountRepository.Add(new Account { AccountId = 2, FirstName = "Liam", LastName = "Neeson" });

            _meterReadingRepository.Add(new MeterReading {AccountId = 1, ReadValue = "00124", ReadingDateTime = _baseline });
            _meterReadingRepository.Add(new MeterReading {AccountId = 2, ReadValue = "00124", ReadingDateTime = _baseline });
            _meterReadingRepository.Add(new MeterReading {AccountId = 3, ReadValue = "00124", ReadingDateTime = _baseline.AddDays(1) });

            _accountRepository.SaveChanges();
            _meterReadingRepository.SaveChanges();
        }

        [Test]
        public void GetAllAccounts_ShouldReturn4SeedAccounts()
        {
            // Arrange

            // Act
            var accounts = _accountRepository.GetAll().ToList();

            // Assert
            Assert.AreEqual(4, accounts.Count);
        }

        [Test]
        public void AddMeterReading_ShouldReturnNull_WhenDuplicateMeterReadingSubmitted()
        {
            // Arrange
            var duplicateMeterReading = new MeterReading
                {AccountId = 1, ReadValue = "00224", ReadingDateTime = _baseline};

            // Act
            var result = _meterReadingRepository.Add(duplicateMeterReading);
            _context.SaveChanges();

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void AddMeterReadings_ShouldStillReturnSomeSuccessful_WhenSomeBadRowsAreProvided()
        {
            // Arrange
            var meterReadings = new List<MeterRowCsvEntity>
            {
                new MeterRowCsvEntity { AccountId = 4, MeterReadValue = "12345", MeterReadingDateTime = _baseline }, // good
                new MeterRowCsvEntity { AccountId = 1, MeterReadValue = "12345", MeterReadingDateTime = _baseline }, // duplicate
                new MeterRowCsvEntity { AccountId = 4, MeterReadValue = "345", MeterReadingDateTime = _baseline }, // bad readvalue
                new MeterRowCsvEntity { AccountId = 3, MeterReadValue = "12345", MeterReadingDateTime = _baseline } // good
            };

            // Act
            var successRows = _energyService.AddMeterReadings(meterReadings);

            // Assert
            Assert.AreEqual(successRows, 2);
        }

        [Test]
        public void AddDuplicateAccount_ShouldReturnFalse()
        {
            // Arrange
            var account = new AccountCsvEntity()
            {
                AccountId = 1,
                FirstName = "Bryan",
                LastName = "Cranston"
            };

            // Act
            var result = _energyService.AddAccount(account);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
