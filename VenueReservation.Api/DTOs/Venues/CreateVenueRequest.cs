using VenueReservation.Api.DTOs.Services;

namespace VenueReservation.Api.DTOs.Venues;

public record CreateVenueRequest(
    string Name,
    int Capacity,
    decimal PricePerHour,
    List<CreateServiceRequest> AvailableServices
);