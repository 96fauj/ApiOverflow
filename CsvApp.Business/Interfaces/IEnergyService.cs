using System.Collections.Generic;

namespace CsvApp.Business.Interfaces
{
    public interface IEnergyService
    {
        IEnumerable<IAccount> GetAllAccounts();
        bool AddMeterReading(IMeterRow reading);
        int AddMeterReadings(IEnumerable<IMeterRow> readings);
        IEnumerable<IMeterRow> GetAllMeterReadings();
    }
}
