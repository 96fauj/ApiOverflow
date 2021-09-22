using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvApp.Business.Interfaces;
using CsvApp.Business.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsvApp.Business.Helpers
{
    public class CsvHelper
    {


        public IEnumerable<TObject> ParseCsvToEnumberable<TMapType, TObject>(Stream stream)
            where TMapType : ClassMap<TObject>
        {
           using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<TMapType>();
                // stream.Close();
                return csv.GetRecords<TObject>().AsEnumerable();
            }
        }
    }

    //public abstract class CsvHelper<TMapType, TObject>
    //    where TMapType : ClassMap<TObject>
    //{
    //    private IEnumerable<TObject> _rows { get; set; }
    //    private IEnumerable<string> _badRows = new List<string>();

    //    public ICsvParseResult<TMapType, TObject> CsvResult()
    //    {
    //        csvReader.Configuration.BadDataFound = context =>
            
    //            _badRows.Add(context.RawRecord);
            

    //        while (csvReader.Read() && !csvReader.Context.Parser.)
    //    }
    //}
}