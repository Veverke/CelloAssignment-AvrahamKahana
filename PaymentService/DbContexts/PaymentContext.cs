using Microsoft.EntityFrameworkCore;
using PaymentService.DbContexts.Models;

namespace PaymentService.DbContexts
{
    public class PaymentContext : DbContext
    {
        public DbSet<ParkingLot> ParkingLots { get; set; }
        public DbSet<TariffPerHour> TariffsPerHour { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //TODO: Move hardcoded settings to appsettings
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Cello;Trusted_Connection=True");
            optionsBuilder.UseSqlServer(@"Server=sql-server,1433;Database=Cello;User Id=sa;Password=StrongPassword123!;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParkingLot>()
                .HasKey(p => p.ParkingLotId);

            modelBuilder.Entity<ParkingLot>()
                .HasMany(p => p.TariffsPerInitialHours)
                .WithOne(p => p.ParkingLot);

            modelBuilder.Entity<TariffPerHour>()
                .HasKey(p => p.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
