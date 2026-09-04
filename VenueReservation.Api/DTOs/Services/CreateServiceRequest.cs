namespace VenueReservation.Api.DTOs.Services;

public record CreateServiceRequest(
    string Name,
    decimal Price
);