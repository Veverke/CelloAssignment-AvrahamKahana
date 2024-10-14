using Microsoft.EntityFrameworkCore;
using ParkingService.DbContexts.Models;

namespace ParkingService.DbContexts
{
    public class ParkingContext : DbContext
    {
        public DbSet<ParkingTransaction> ParkingTransactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //TODO: Move hardcoded settings to appsettings
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Cello;Trusted_Connection=True");
            optionsBuilder.UseSqlServer(@"Server=sql-server,1433;Database=Cello;User Id=sa;Password=StrongPassword123!;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParkingTransaction>()
                .HasKey(p => p.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
