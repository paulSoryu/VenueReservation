
using Microsoft.EntityFrameworkCore;
using VenueReservation.Application.Features.Venues.DTOs;
using VenueReservation.Application.Interfaces;
using VenueReservation.Domain.Models.Venues;
using VenueReservation.Infrastructure.Data;

namespace VenueReservation.Infrastructure.Data.Repositories;

public class VenueRepository(ApplicationDbContext context) : IVenueRepository
{
    public async Task AddAsync(Venue venue, CancellationToken cancellationToken)
    {
        await context.Venues.AddAsync(venue, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Venue?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Venues
            .Include(v => v.AvailableServices)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<Venue?> GetByIdNoFiltersAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Venues
            .IgnoreQueryFilters()
            .Include(v => v.AvailableServices)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public void SoftDelete(Venue venue)
    {
        venue.SoftDelete();
    }

    public void HardDelete(Venue venue)
    {
        context.Venues.Remove(venue);
    }

    public async Task<bool> HasAnyReservationsAsync(Guid venueId, CancellationToken cancellationToken)
    {
        return await context.Reservations
            .AsNoTracking()
            .AnyAsync(r =>
                r.VenueId == venueId &&
                r.Period.StartTime.AddHours(r.Period.DurationInHours) > DateTime.UtcNow,
                cancellationToken);
    }

    public async Task<List<SearchVenuesResponse>> SearchAvailableVenuesAsync(
    int capacity,
    List<Guid> excludedVenueIds,
    CancellationToken cancellationToken)
    {
        return await context.Venues
            .AsNoTracking()
            .Where(v => v.Capacity >= capacity)
            .Where(v => !excludedVenueIds.Contains(v.Id))
            .Select(v => new SearchVenuesResponse(
                v.Id,
                v.Name.Value,
                v.Capacity.Value,
                v.BasePricePerHour.Value,
                v.AvailableServices.Select(s => new AvailableServiceResponseDto(
                    s.Id,
                    s.Name.Value,
                    s.Price.Value
                )).ToList()
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Venue>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Venues
            .IgnoreQueryFilters() // Allows fetching venues that might be soft-deleted or filtered out by global query filters
            .AsNoTracking()
            .Include(v => v.AvailableServices)
            .ToListAsync(cancellationToken);
    }
}