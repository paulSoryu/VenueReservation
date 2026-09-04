
using VenueReservation.Domain.Results;
using MediatR;

namespace VenueReservation.Application.Features.Venues.Commands.DeleteVenue;

public record DeleteVenueCommand(Guid Id, bool IsSoftDelete = true) : IRequest<Result>;