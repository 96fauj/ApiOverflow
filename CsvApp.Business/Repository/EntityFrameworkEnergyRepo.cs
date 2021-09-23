using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsvApp.Business.Parsers;
using EnergyDataLayer.Context;

namespace CsvApp.Business.Repository
{
    public class EntityframeworkEnergyRepo
    {
        private EnergyDbContext _context;
    }

    public static class EntityFrameworkEnergyRepoExtensions
    {
        public static void SeedAccounts(this EnergyDbContext context, IEnumerable<Account> accounts)
        {
            using (context)
            {
                if (context.Accounts.Any())
                {
                    return; // data already seeded
                }

                context.Accounts.AddRange(accounts);
                context.SaveChanges();
            }
        }
    }
}
