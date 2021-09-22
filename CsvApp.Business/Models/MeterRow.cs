using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CsvApp.Business.Helpers;
using CsvApp.Business.Interfaces;
using CsvHelper.Configuration;

namespace CsvApp.Business.Models
{
    public class MeterRow : IUniqueCsvEntity
    {
        [CsvIdentifier]
        public int AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }
    }

    public sealed class MeterRowMap : ClassMap<MeterRow>
    {
        public MeterRowMap()
        {
            var dateTimeConverter = new CsvDateTimeConverter(MeterReadConfigSettings.DateFormat);

            Map(m => m.AccountId);
            Map(m => m.MeterReadingDateTime).TypeConverterOption.Format(MeterReadConfigSettings.DateFormat);
            //Map(m => m.MeterReadingDateTime).TypeConverter(dateTimeConverter);

            Map(m => m.MeterReadValue).Validate(v =>
            {
                var isCorrectFormat = Regex.IsMatch(v.Field, MeterReadConfigSettings.ReadValueRegex);
                return isCorrectFormat && !string.IsNullOrEmpty(v.Field);
            });
        }
    }

    public class MeterRowParser : IdentityCsvParser<MeterRow, int>
    {
        internal override IEnumerable<ClassMap> ClassMaps => new List<ClassMap>()
        {
            new MeterRowMap()
        };
    }
}
