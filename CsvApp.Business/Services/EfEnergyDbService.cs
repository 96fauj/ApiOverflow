using System.Collections.Generic;
using System.Linq;
using CsvApp.Business.Interfaces;
using CsvApp.Business.Parsers;
using EnergyDataLayer.Context;
using EnergyDataLayer.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CsvApp.Business.Services
{
    public class EfEnergyDbService : IEnergyService
    {
        private readonly DbContext _context;
        private readonly Repository<MeterReading> _meterReadingRepo;
        private readonly Repository<Account> _accountRepo;

        public EfEnergyDbService(DbContext context)
        {
            _context = context;
            _meterReadingRepo = new Repository<MeterReading>(_context);
            _accountRepo = new Repository<Account>(_context);
        }

        public bool AddMeterReading(IMeterRow reading)
        {
            var dbEntity = _meterReadingRepo.Add(MapIMeterRowToMeterReading(reading));
            _context.SaveChanges();
            return dbEntity != null;
        }

        public int AddMeterReadings(IEnumerable<IMeterRow> readings)
        {
            var dbEntities = _meterReadingRepo.AddRange(readings.Select(r => MapIMeterRowToMeterReading(r)));
            _context.SaveChanges();
            
            return dbEntities.Count();
        }

        public bool AddAccount(IAccount account)
        {
            var dbEntity = _accountRepo.Add(MapIAccountToAccount(account));
            _context.SaveChanges();
            return dbEntity != null;
        }

        private MeterReading MapIMeterRowToMeterReading(IMeterRow reading)
        {
            return new MeterReading
            {
                AccountId = reading.AccountId,
                ReadingDateTime = reading.MeterReadingDateTime,
                ReadValue = reading.MeterReadValue
            };
        }

        private Account MapIAccountToAccount(IAccount account)
        {
            return new Account
            {
                AccountId = account.AccountId,
                FirstName = account.FirstName,
                LastName = account.LastName
            };
        }

        public IEnumerable<IMeterRow> GetAllMeterReadings()
        {
            return _meterReadingRepo.GetAll().Select(a => new MeterRowCsvEntity()
            {
                AccountId = a.AccountId,
                MeterReadValue = a.ReadValue,
                MeterReadingDateTime = a.ReadingDateTime
            });
        }

        public IEnumerable<IAccount> GetAllAccounts()
        {
            return _accountRepo.GetAll().Select(a => new AccountCsvEntity()
            {
                AccountId = a.AccountId,
                FirstName = a.FirstName,
                LastName = a.LastName
            });
        }
    }

    public static class EntityFrameworkEnergyRepoExtensions
    {
        public static void SeedAccounts(this EnergyDbContext context, IEnumerable<Account> accounts)
        {
            using (context)
            {
                if (context.Accounts.Any())
                {
                    return; // data already seeded
                }

                context.Accounts.AddRange(accounts);
                context.SaveChanges();
            }
        }
    }
}
