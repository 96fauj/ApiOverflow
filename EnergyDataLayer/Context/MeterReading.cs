using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnergyDataLayer.Context
{
    public class MeterReading
    {
        [Key]
        [Required]
        public int AccountId { get; set; }

        [Required]
        public DateTime ReadingDateTime { get; set; }

        [Required]
        [MaxLength(5), MinLength(5)]
        public string ReadValue { get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account {  get; set; }
    }
}
