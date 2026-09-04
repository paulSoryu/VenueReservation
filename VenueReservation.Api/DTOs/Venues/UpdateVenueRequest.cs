using VenueReservation.Api.DTOs.Services;

namespace VenueReservation.Api.DTOs.Venues;

public record UpdateVenueRequest(
    string Name,
    int Capacity,
    decimal BasePricePerHour,
    List<UpdateServiceRequest> AvailableServices
);