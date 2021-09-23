using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnergyDataLayer.Context
{
    public class MeterReading
    {
        [Key]
        public int AccountId { get; set; }
        public DateTime ReadingDateTime { get; set; }
        public int ReadValue { get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account {  get; set; }
    }
}
