using Microsoft.EntityFrameworkCore;

namespace EnergyDataLayer.Context
{
    public class EnergyDbContext : DbContext
    {
        public EnergyDbContext(DbContextOptions<EnergyDbContext> options)
        : base(options) { }

        public DbSet<Account> Accounts {  get; set;}
        public DbSet<MeterReading> MeterReadings {  get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var accountEntity = modelBuilder.Entity<Account>();

            accountEntity
                .HasKey(a => a.AccountId);
            accountEntity
                .HasMany(a => a.MeterReadings)
                .WithOne(r => r.Account)
                .HasForeignKey(r => r.AccountId)
                .IsRequired();


            var meterReadEntity = modelBuilder.Entity<MeterReading>();

            meterReadEntity
                .HasKey(r => new { r.AccountId, r.ReadingDateTime});
            meterReadEntity
                .HasIndex(r => r.AccountId);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
 