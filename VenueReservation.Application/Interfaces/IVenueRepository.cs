
using VenueReservation.Application.Features.Venues.DTOs;
using VenueReservation.Domain.Models.Venues;

namespace VenueReservation.Application.Interfaces;

public interface IVenueRepository
{
    Task AddAsync(Venue venue, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<Venue?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Venue?> GetByIdNoFiltersAsync(Guid id, CancellationToken cancellationToken);
    void SoftDelete(Venue venue);
    void HardDelete(Venue venue);
    Task<bool> HasAnyReservationsAsync(Guid venueId, CancellationToken cancellationToken);
    Task<List<SearchVenuesResponse>> SearchAvailableVenuesAsync(
        int capacity,
        List<Guid> excludedVenueIds,
        CancellationToken cancellationToken);
    Task<List<Venue>> GetAllAsync(CancellationToken cancellationToken);
}