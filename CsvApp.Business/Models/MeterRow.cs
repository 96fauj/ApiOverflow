using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using CsvApp.Business.Helpers;
using CsvApp.Business.Interfaces;
using CsvHelper;
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
        //public static MeterReadConfigSettings ConfigSettings { set; get; }

        public MeterRowMap()
        {
            // var configSettings = serviceProvider.GetService<MeterReadConfigSettings>();

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

    public class MeterParseResult : CsvParseResult<MeterRow, int>
    {
        public IEnumerable<MeterRow> GoodRows { get; set; }
        public IEnumerable<string> BadRows { get; set; }
        public int Successful { get; set; }
        public int Failed { get; set; }
    }

    public class MeterRowParser
    {
        private Dictionary<int, MeterRow> _rows = new Dictionary<int, MeterRow>();
        private List<string> _badRows = new List<string>();
        //private readonly MeterReadConfigSettings _configSettings;

        //public MeterRowParser(MeterReadConfigSettings configSettings)
        //{
            
        //}

        public MeterParseResult ParseCsv(StreamReader streamReader)
        {
            var csvReaderConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                ReadingExceptionOccurred = ex =>
                {
                   _badRows.Add(ex.Exception.Context.Parser.RawRecord);
                    //_badRows.Add($"{ex.Exception.Context.Parser.RawRecord} // error {ex.Exception.Message}");
                    return false;
                },
                SanitizeForInjection = true // todo - review if future fields may require
            };

            using (var csvReader = new CsvReader(streamReader, csvReaderConfig))
            {
                var map = new MeterRowMap();
                
                csvReader.Context.RegisterClassMap(map);

                while (csvReader.Read())
                {
                    var record = csvReader.GetRecord<MeterRow>();
                    if (record == null)
                    {
                        // this row didn't parse and has already been handled by the reading exception callback
                        continue; 
                    }

                    if (/*record == null ||*/
                        _rows.ContainsKey(record.AccountId))
                        //!IsValidColumnCount(csvReader.Parser.Count, csvReader.HeaderRecord.Length))
                    {
                        _badRows.Add(csvReader.Parser.RawRecord);
                    }
                    else
                    {
                        _rows.Add(record.AccountId, record);
                    }
                }
                // end
            }

            return new MeterParseResult()
            {
                Successful = _rows.Count,
                Failed = _badRows.Count,
                GoodRows = _rows.Values,
                BadRows = _badRows
            };
        }

        private static bool IsValidColumnCount(int recordColumns, int headerColumnCount)
        {
            return recordColumns == headerColumnCount;
        }
    }
}
