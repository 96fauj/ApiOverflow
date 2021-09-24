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
            this.Completed = new Dictionary<TIdentifierType, TCsvEntity>();
            this.Failed = new List<string>();
        }

        [JsonIgnore]
        public IDictionary<TIdentifierType, TCsvEntity> Completed { get; set; }

        [JsonIgnore]
        public List<string> Failed { get; set; }

        public int SuccessfulCount => this.Completed.Count;
        public int FailedCount => this.Failed.Count;
    }
}
