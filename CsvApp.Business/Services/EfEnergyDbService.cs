using System.Collections.Generic;
using System.Linq;
using CsvApp.Business.Interfaces;
using CsvApp.Business.Parsers;
using EnergyDataLayer.Context;
using EnergyDataLayer.Helpers;

namespace CsvApp.Business.Services
{
    public class EfEnergyDbService : IEnergyRepo
    {
        private readonly EnergyDbContext _context;

        public EfEnergyDbService(EnergyDbContext context)
        {
            _context = context;
        }

        public bool AddMeterReading(IMeterRow reading)
        {
            var meterReadingRepo = new Repository<MeterReading>(_context);
            var dbEntity = meterReadingRepo.Add(MapIMeterRowToMeterReading(reading));
            _context.SaveChanges();
            return dbEntity != null;
        }

        public int AddMeterReadings(IEnumerable<IMeterRow> readings)
        {
            var meterReadingRepo = new Repository<MeterReading>(_context);
            var dbEntities = meterReadingRepo.AddRange(readings.Select(r => MapIMeterRowToMeterReading(r)));
            _context.SaveChanges();
            
            //return _context.SaveResult.SuccessfulAdds;
            return dbEntities.Count();
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

        public IEnumerable<IMeterRow> GetAllMeterReadings()
        {
            return _context.MeterReadings.Select(a => new MeterRowCsvEntity()
            {
                AccountId = a.AccountId,
                MeterReadValue = a.ReadValue,
                MeterReadingDateTime = a.ReadingDateTime
            });
        }

        IEnumerable<IAccount> IEnergyRepo.GetAllAccounts()
        {
            return _context.Accounts.Select(a => new AccountCsvEntity()
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
