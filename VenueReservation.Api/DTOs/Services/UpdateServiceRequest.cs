namespace VenueReservation.Api.DTOs.Services;

public record UpdateServiceRequest(
    Guid? Id, // Nullable, because it might not be provided for new services
    string Name,
    decimal Price
);
