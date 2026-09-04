using MediatR;
using VenueReservation.Application.Features.Venues.DTOs;
using VenueReservation.Application.Interfaces;
using VenueReservation.Domain.Models.Reservations.ValueObjects;
using VenueReservation.Domain.Results;

namespace VenueReservation.Application.Features.Venues.Queries.SearchVenues;

public class SearchVenuesQueryHandler(
    IVenueRepository venueRepository,
    IReservationRepository reservationRepository)
    : IRequestHandler<SearchVenuesQuery, Result<List<SearchVenuesResponse>>>
{
    public async Task<Result<List<SearchVenuesResponse>>> Handle(
        SearchVenuesQuery query, 
        CancellationToken cancellationToken)
    {
        var period = BookingPeriod.Create(query.DateTime, query.DurationInHours).Value;

        var bookedVenueIds = await reservationRepository.GetBookedVenueIdsAsync(
            period.StartTime,
            period.EndTime,
            cancellationToken);

        var availableVenues = await venueRepository.SearchAvailableVenuesAsync(
            query.Capacity,
            bookedVenueIds,
            cancellationToken);

        return Result<List<SearchVenuesResponse>>.Success(availableVenues);
    }
}