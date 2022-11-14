using Microsoft.EntityFrameworkCore;
using VacationRental.Core.Domain;

namespace VacationRental.Data;

public class VacationRentalDbContext : DbContext
{
    public VacationRentalDbContext(DbContextOptions<VacationRentalDbContext> options) : base(options) { }

    public DbSet<Rental> Rentals { get; set; }

    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rental>();
        modelBuilder.Entity<Booking>();
    }
}