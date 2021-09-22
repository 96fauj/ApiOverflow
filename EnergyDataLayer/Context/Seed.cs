using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EnergyDataLayer.Context
{
    public class DataSetup
    {
        public static void SeedAccounts(IServiceProvider serviceProvider, IEnumerable<Account> accounts)
        {
            using (var context = new EnergyDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<EnergyDbContext>>()))
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
