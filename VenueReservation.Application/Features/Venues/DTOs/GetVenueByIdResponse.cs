
using VenueReservation.Domain.Models.Venues;

namespace VenueReservation.Application.Features.Venues.DTOs;

public record GetVenueByIdResponse(
    Guid Id,
    string Name,
    int Capacity,
    decimal PricePerHour,
    List<GetVenueByIdServiceDto> AvailableServices
)
{
    public static GetVenueByIdResponse FromDomain(Venue venue)
    {
        return new GetVenueByIdResponse(
            venue.Id,
            venue.Name.Value,          
            venue.Capacity.Value,      
            venue.BasePricePerHour.Value,
            venue.AvailableServices.Select(s => new GetVenueByIdServiceDto(
                s.Id,
                s.Name.Value,
                s.Price.Value
            )).ToList()
        );
    }
}

public record GetVenueByIdServiceDto(
    Guid Id,
    string Name,
    decimal Price
);