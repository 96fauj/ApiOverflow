using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using CsvApp.Business.Helpers;
using CsvApp.Business.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsvApp.Business.Parsers
{
    public abstract class IdentityCsvParser<TCsvEntity, TIdentifierType>
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
                    _result.Failed.Add(ex.Exception.Context.Parser.RawRecord);
                    //_badRows.Add($"{ex.Exception.Context.Parser.RawRecord} // error {ex.Exception.Message}");
                    return false;
                },
                HeaderValidated = context =>
                {
                    if (context.InvalidHeaders.Any())
                    {
                        throw new ArgumentException("Csv Headers invalid, check the file uploaded");
                    }
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
                        _result.Failed.Add(csvReader.Parser.RawRecord);
                    }
                    else
                    {
                        _result.Completed.Add((TIdentifierType)record.GetIdentifier(), record);
                    }
                }
            }

            return this._result;
        }

        public CsvParseResult<TCsvEntity, TIdentifierType> ParseCsvFromResourceFile(string resourceLocation)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            
            using (Stream stream = assembly.GetManifestResourceStream(resourceLocation))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return this.ParseCsv(reader);
                }
            }
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

            return _result.Completed.ContainsKey(key);
        }
    }
}