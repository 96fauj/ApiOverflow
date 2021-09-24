using System;
using System.Collections.Generic;

namespace CsvApp.Business.Interfaces
{
    public interface IEnergyRepo
    {
        IEnumerable<IAccount> GetAllAccounts();
        bool AddMeterReading(IMeterRow reading);
        int AddMeterReadings(IEnumerable<IMeterRow> readings);
        IEnumerable<IMeterRow> GetAllMeterReadings();
    }

    public interface IAccount : IUniqueCsvEntity
    {
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public interface IMeterRow : IUniqueCsvEntity
    {
        public int AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }
    }

}
