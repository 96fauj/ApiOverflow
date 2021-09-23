using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CsvApp.Business.Helpers;
using CsvApp.Business.Interfaces;
using CsvHelper.Configuration;

namespace CsvApp.Business.Models
{
    public class MeterRowCsvEntity : IUniqueCsvEntity
    {
        [CsvIdentifier]
        public int AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }
    }

    public sealed class MeterRowMap : ClassMap<MeterRowCsvEntity>
    {
        public MeterRowMap()
        {
            Map(m => m.AccountId);
            Map(m => m.MeterReadingDateTime).TypeConverterOption.Format(MeterReadSettings.Settings.DateFormat);
            Map(m => m.MeterReadValue).Validate(v =>
            {
                var isCorrectFormat = Regex.IsMatch(v.Field, MeterReadSettings.Settings.ReadValueRegex);
                return isCorrectFormat && !string.IsNullOrEmpty(v.Field);
            });
        }
    }

    public class MeterRowParser : IdentityCsvParser<MeterRowCsvEntity, int>
    {
        internal override IEnumerable<ClassMap> ClassMaps => new List<ClassMap>()
        {
            new MeterRowMap()
        };
    }
}
