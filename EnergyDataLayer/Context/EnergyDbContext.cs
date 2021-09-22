using Microsoft.EntityFrameworkCore;

namespace EnergyDataLayer.Context
{
    public class EnergyDbContext : DbContext
    {
        public EnergyDbContext(DbContextOptions<EnergyDbContext> options)
        : base(options) { }

        public DbSet<Account> Accounts {  get; set;}
        public DbSet<MeterReading> MeterReadings {  get; set;}
    }
}
