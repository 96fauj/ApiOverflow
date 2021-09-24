using System;

namespace CsvApp.Business.Interfaces
{
    public interface IMeterRow : IUniqueCsvEntity
    {
        public int AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }
    }
}