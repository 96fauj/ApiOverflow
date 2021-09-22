using System;

namespace EnergyDataLayer.Context
{
    public class MeterReading
    {
        public int AccountId { get; set; }
        public DateTime ReadingDateTime { get; set; }
        public int ReadValue { get; set; }
    }
}
