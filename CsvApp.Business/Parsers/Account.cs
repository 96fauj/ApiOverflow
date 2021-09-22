using System.Collections.Generic;
using CsvApp.Business.Helpers;
using CsvApp.Business.Interfaces;
using CsvHelper.Configuration;

namespace CsvApp.Business.Parsers
{
    public class Account : IUniqueCsvEntity
    {
        [CsvIdentifier]
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public sealed class AccountMap : ClassMap<Account>
    {
        public AccountMap()
        {
            Map(m => m.AccountId);
            Map(m => m.FirstName);
            Map(m => m.LastName);
        }
    }

    public class AccountRowParser : IdentityCsvParser<Account, int>
    {
        internal override IEnumerable<ClassMap> ClassMaps => new List<ClassMap>()
        {
            new AccountMap()
        };
    }
}
