using System.Collections.Generic;
using System.Text.Json.Serialization;
using CsvApp.Business.Interfaces;

namespace CsvApp.Business.Helpers
{
    public class CsvParseResult<TCsvEntity, TIdentifierType> 
        where TCsvEntity : IUniqueCsvEntity
    {
        public CsvParseResult()
        {
            this.GoodRows = new Dictionary<TIdentifierType, TCsvEntity>();
            this.BadRows = new List<string>();
        }

        [JsonIgnore]
        public IDictionary<TIdentifierType, TCsvEntity> GoodRows { get; set; }

        [JsonIgnore]
        public List<string> BadRows { get; set; }

        public int Successful => this.GoodRows.Count;
        public int Failed => this.BadRows.Count;
    }
}
