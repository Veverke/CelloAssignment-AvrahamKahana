using Microsoft.EntityFrameworkCore;
using PaymentService.DbContexts.Models;

namespace PaymentService.DbContexts
{
    public class PaymentContext : DbContext
    {
        public DbSet<ParkingLot> ParkingLots { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //TODO: Move hardcoded settings to appsettings
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Cello;Trusted_Connection=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParkingLot>()
                .HasKey(p => p.ParkingLotId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
