using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvApp.Business.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsvApp.Business.Helpers
{
    public abstract class CsvHelper<TCsvEntity, TIdentifierType>
        where TCsvEntity : IUniqueCsvEntity
    {
        private CsvParseResult<TCsvEntity, TIdentifierType> _result = new CsvParseResult<TCsvEntity, TIdentifierType>();
        internal abstract IEnumerable<ClassMap> ClassMaps { get; }

        private CsvConfiguration CsvConfig()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                ReadingExceptionOccurred = ex =>
                {
                    _result.BadRows.Add(ex.Exception.Context.Parser.RawRecord);
                    //_badRows.Add($"{ex.Exception.Context.Parser.RawRecord} // error {ex.Exception.Message}");
                    return false;
                },
                SanitizeForInjection = true // todo - review if future fields may require
            };
        }

        public CsvParseResult<TCsvEntity, TIdentifierType> ParseCsv(StreamReader streamReader)
        {
            using (var csvReader = new CsvReader(streamReader, CsvConfig()))
            {
                SetupClassMaps(csvReader);

                while (csvReader.Read())
                {
                    var record = csvReader.GetRecord<TCsvEntity>();
                    if (record == null)
                    {
                        // this row didn't parse and has already been handled by the reading exception callback
                        continue;
                    }
                    
                    if (IsDuplicateRow(record))
                    //!IsValidColumnCount(csvReader.Parser.Count, csvReader.HeaderRecord.Length))
                    {
                        _result.BadRows.Add(csvReader.Parser.RawRecord);
                    }
                    else
                    {
                        _result.GoodRows.Add((TIdentifierType)record.GetIdentifier(), record);
                    }
                }
            }

            return this._result;
        }

        private void SetupClassMaps(CsvReader csvReader)
        {
            foreach (var classMap in ClassMaps)
            {
                csvReader.Context.RegisterClassMap(classMap);
            }
        }

        private bool IsDuplicateRow(TCsvEntity csvEntity)
        {
            var key = (TIdentifierType)csvEntity.GetIdentifier();

            return _result.GoodRows.ContainsKey(key);
        }
    }
}