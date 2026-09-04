
using VenueReservation.Domain.Results;
using MediatR;

namespace VenueReservation.Application.Features.Venues.Commands.UpdateVenue;

public record UpdateVenueCommand(
    Guid Id,
    string Name,
    int Capacity,
    decimal BasePricePerHour,
    List<UpdateServiceCommandDto> AvailableServices
) : IRequest<Result>;

public record UpdateServiceCommandDto(
    Guid? Id,
    string Name,
    decimal Price
);