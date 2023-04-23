using Microsoft.EntityFrameworkCore;
using MobileCompaniesApi.Models;
using System.Diagnostics.Metrics;

namespace MobileCompaniesApi.Data
{
    public class MobileCoContext : DbContext
    {
        public MobileCoContext(DbContextOptions<MobileCoContext> options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Antenna> Antennas { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceUserPhoto> DeviceUserPhotos { get; set; }
        public DbSet<Position> Positions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Unique Index for Company Name
            modelBuilder.Entity<Company>()
                .HasIndex(c => c.Name)
                .IsUnique();

            // Unique Composite Index for the Coordinates
            modelBuilder.Entity<Antenna>()
                .HasIndex(a => new { a.Latitude, a.Longitude })
                .IsUnique();

            // Unique Composite Index for the Device Info
            modelBuilder.Entity<Device>()
                .HasIndex(d => new { d.Name, d.Model, d.Manufacturer, d.Type })
                .IsUnique();

            // You can't delete a Company that has Antennas
            modelBuilder.Entity<Company>()
                .HasMany(t => t.Antennas)
                .WithOne(l => l.Company)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
