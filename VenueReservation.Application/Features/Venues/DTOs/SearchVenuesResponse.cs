
namespace VenueReservation.Application.Features.Venues.DTOs;

public record SearchVenuesResponse(
    Guid Id,
    string Name,
    int Capacity,
    decimal BasePricePerHour,
    List<AvailableServiceResponseDto> AvailableServices
);

public record AvailableServiceResponseDto(
    Guid Id,
    string Name,
    decimal Price
);