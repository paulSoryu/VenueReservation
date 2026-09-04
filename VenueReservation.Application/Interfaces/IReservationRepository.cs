
using VenueReservation.Domain.Models.Reservations;

namespace VenueReservation.Application.Interfaces;

public interface IReservationRepository
{
    Task<List<Guid>> GetBookedVenueIdsAsync(DateTime start, DateTime end, CancellationToken cancellationToken);
    Task AddAsync(Reservation reservation, CancellationToken cancellationToken);
    Task<List<Reservation>> GetReservationsInPeriodAsync(DateOnly from, DateOnly to, CancellationToken cancellationToken);
}