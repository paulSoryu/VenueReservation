
using MediatR;
using VenueReservation.Application.Features.Venues.DTOs;
using VenueReservation.Domain.Results;

namespace VenueReservation.Application.Features.Venues.Commands.CreateVenue;

public record CreateVenueCommand(
    string Name,
    int Capacity,
    decimal PricePerHour,
    List<CreateServiceCommandDto> AvailableServices
) : IRequest<Result<GetVenueByIdResponse>>;

public record CreateServiceCommandDto(
    string Name,
    decimal Price
);