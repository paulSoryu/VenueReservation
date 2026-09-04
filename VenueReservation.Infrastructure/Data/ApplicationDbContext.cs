using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VenueReservation.Domain.Models.Reservations;
using VenueReservation.Domain.Models.Services;
using VenueReservation.Domain.Models.Venues;

namespace VenueReservation.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Venue> Venues => Set<Venue>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Service> Services => Set<Service>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
