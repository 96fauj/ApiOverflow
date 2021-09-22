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
    public class MeterRow
    {
        public int AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }
    }

    public sealed class MeterRowMap : ClassMap<MeterRow>
    {
        public MeterRowMap(MeterReadConfigSettings configSettings)
        {
            var dateTimeConverter = new CsvDateTimeConverter(configSettings.DateFormat);

            Map(m => m.AccountId);
            Map(m => m.MeterReadingDateTime).TypeConverterOption.Format(configSettings.DateFormat);
            //Map(m => m.MeterReadingDateTime).TypeConverter(dateTimeConverter);

            Map(m => m.MeterReadValue).Validate(v =>
            {
                var isCorrectFormat = Regex.IsMatch(v.Field, @configSettings.ReadValueRegex);
                return isCorrectFormat && !string.IsNullOrEmpty(v.Field);
            });
        }
    }

    public class MeterParseResult : ICsvParseResult<MeterRowMap, MeterRow>
    {
        public IEnumerable<MeterRow> GoodRows { get; set; }
        public IEnumerable<string> BadRows { get; set; }
        public int Successful { get; set; }
        public int Failed { get; set; }
    }

    public class MeterRowParse
    {
        private Dictionary<int, MeterRow> _rows = new Dictionary<int, MeterRow>();
        private List<string> _badRows = new List<string>();
        private readonly MeterReadConfigSettings _configSettings;
        private readonly StreamReader _streamReader;

        public MeterRowParse(StreamReader streamReader, MeterReadConfigSettings configSettings)
        {
            _configSettings = configSettings;
            _streamReader = streamReader;
        }

        public MeterParseResult ParseCsv()
        {
            var isRowValid = true;

            var csvReaderConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                BadDataFound = arg =>
                {
                    isRowValid = false;
                },
                MissingFieldFound = arg =>
                {
                    isRowValid = false;
                },
                ReadingExceptionOccurred = ex =>
                {
                    isRowValid = false;

                    //_badRows.Add($"{ex.Exception.Context.Parser.RawRecord} // error {ex.Exception.Message}");
                    return false;
                },
                SanitizeForInjection = true // todo - review if future fields may require
            };

            using (var csvReader = new CsvReader(_streamReader, csvReaderConfig))
            {
                var map = new MeterRowMap(_configSettings);
                csvReader.Context.RegisterClassMap(map);

                while (csvReader.Read())
                {
                    var record = csvReader.GetRecord<MeterRow>();

                    if (record == null ||
                        _rows.ContainsKey(record.AccountId) ||
                        isRowValid ||
                        !IsValidColumnCount(csvReader.Parser.Count, csvReader.HeaderRecord.Length))
                    {
                        _badRows.Add(csvReader.Parser.RawRecord);
                    }
                    else
                    {
                        _rows.Add(record.AccountId, record);
                    }

                    isRowValid = true;
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

        private bool IsValidColumnCount(int recordColumns, int headerColumnCount)
        {
            return recordColumns == headerColumnCount;
        }
    }
}
