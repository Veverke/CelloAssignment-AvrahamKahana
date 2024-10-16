﻿using Microsoft.EntityFrameworkCore;
using PaymentService.DbContexts.Models;

namespace PaymentService.DbContexts
{
    public class PaymentContext : DbContext
    {
        public DbSet<ParkingLot> ParkingLots { get; set; }
        public DbSet<TariffPerHour> TariffsPerHour { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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

            modelBuilder.HasSequence<int>("InvoiceIdSequence");

            modelBuilder.Entity<Invoice>()
                .Property(i => i.Id)
                .HasDefaultValueSql("NEXT VALUE FOR InvoiceIdSequence")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Invoice>()
                .HasKey(p => new { p.CustomerId, p.ParkingLotId, p.GateOpened, p.GateClosed });

            base.OnModelCreating(modelBuilder);
        }
    }
}
