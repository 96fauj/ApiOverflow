using System.Collections.Generic;

namespace CsvApp.Business.Interfaces
{
    public interface IEnergyService
    {
        IEnumerable<IAccount> GetAllAccounts();
        IEnumerable<IMeterRow> GetAllMeterReadings();
        bool AddMeterReading(IMeterRow reading);
        int AddMeterReadings(IEnumerable<IMeterRow> readings);
        bool AddAccount(IAccount account);
    }
}
