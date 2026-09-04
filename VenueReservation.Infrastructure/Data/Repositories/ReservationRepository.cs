
using Microsoft.EntityFrameworkCore;
using VenueReservation.Application.Interfaces;
using VenueReservation.Domain.Models.Reservations;

namespace VenueReservation.Infrastructure.Data.Repositories;

public class ReservationRepository(ApplicationDbContext context) : IReservationRepository
{
    public async Task<List<Guid>> GetBookedVenueIdsAsync(DateTime start, DateTime end, CancellationToken cancellationToken)
    {
        return await context.Reservations
            .AsNoTracking()
            .Where(r =>
                r.Period.StartTime < end &&
                r.Period.DurationInHours > (start - r.Period.StartTime).TotalHours)
            .Select(r => r.VenueId)
            .Distinct()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Reservation reservation, CancellationToken cancellationToken)
    {
        await context.Reservations.AddAsync(reservation, cancellationToken);
    }

    public async Task<List<Reservation>> GetReservationsInPeriodAsync(DateOnly from, DateOnly to, CancellationToken cancellationToken)
    {
        return await context.Reservations
            .IgnoreQueryFilters() // Allows fetching reservations that might be soft-deleted or filtered out by global query filters
            .AsNoTracking()
            .Include(r => r.SelectedServices)
            .Where(r => DateOnly.FromDateTime(r.Period.StartTime) >= from &&
                        DateOnly.FromDateTime(r.Period.StartTime) <= to)
            .ToListAsync(cancellationToken);
    }
}