using System.Collections.Generic;
using CsvApp.Business.Models;
using CsvHelper.Configuration;

namespace CsvApp.Business.Interfaces
{
    public interface ICsvParseResult<TMapType, TObject>
        where TMapType : ClassMap<TObject>
    {
        IEnumerable<TObject> GoodRows { get; set; }
        IEnumerable<string> BadRows { get; set; }
    }
}
