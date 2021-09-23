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
            modelBuilder.Entity<Account>()
                .HasKey(a => a.AccountId);
            modelBuilder.Entity<Account>()
                .HasMany(a => a.MeterReadings)
                .WithOne(r => r.Account)
                .HasForeignKey(r => r.AccountId)
                .IsRequired();

            modelBuilder.Entity<MeterReading>()
                .HasKey(r => new { r.AccountId, r.ReadingDateTime});
            modelBuilder.Entity<MeterReading>()
                .HasIndex(r => r.AccountId);
                

            base.OnModelCreating(modelBuilder);
        }
    }
}
 