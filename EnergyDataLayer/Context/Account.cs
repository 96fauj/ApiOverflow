using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnergyDataLayer.Context
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName {  get; set; }

        public ICollection<MeterReading> MeterReadings { get; set; }
    }
}
