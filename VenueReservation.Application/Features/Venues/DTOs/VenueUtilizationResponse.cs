
namespace VenueReservation.Application.Features.Venues.DTOs;

public record VenueUtilizationResponse(
    Guid VenueId,
    string VenueName,
    int TotalBookedHours,
    double UtilizationPercentage
);