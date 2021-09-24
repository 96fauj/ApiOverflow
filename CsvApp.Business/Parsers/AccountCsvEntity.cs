using System.Collections.Generic;
using System.Linq;
using CsvApp.Business.Helpers;
using CsvApp.Business.Interfaces;
using CsvHelper.Configuration;
using EnergyDataLayer.Context;

namespace CsvApp.Business.Parsers
{
    public class AccountCsvEntity : IAccount
    {
        [CsvIdentifier]
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public sealed class AccountMap : ClassMap<AccountCsvEntity>
    {
        public AccountMap()
        {
            Map(m => m.AccountId);
            Map(m => m.FirstName);
            Map(m => m.LastName);
        }
    }

    public class AccountRowParser : IdentityCsvParser<AccountCsvEntity, int>
    {
        internal override IEnumerable<ClassMap> ClassMaps => new List<ClassMap>()
        {
            new AccountMap()
        };

        public IEnumerable<Account> CsvEntityToAccounts(IEnumerable<AccountCsvEntity> csvEntities)
        {
            // automapper is probably overkill for the scope of this project
            var accounts = csvEntities.Select(e => new Account
            {
                AccountId = e.AccountId,
                FirstName = e.FirstName,
                LastName = e.LastName
            });

            return accounts;
        }
    }
}
