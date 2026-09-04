
using VenueReservation.Domain.Results;
using MediatR;
using VenueReservation.Application.Features.Venues.DTOs;

namespace VenueReservation.Application.Features.Venues.Queries.SearchVenues;

public record SearchVenuesQuery(
    DateTime DateTime,
    int DurationInHours,
    int Capacity
) : IRequest<Result<List<SearchVenuesResponse>>>;
