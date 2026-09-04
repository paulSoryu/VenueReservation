
using MediatR;
using VenueReservation.Application.Features.Venues.DTOs;
using VenueReservation.Domain.Results;

namespace VenueReservation.Application.Features.Venues.Queries.GetVenueById;

public record GetVenueByIdQuery(Guid Id) : IRequest<Result<GetVenueByIdResponse>>;