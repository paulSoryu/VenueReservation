using System.ComponentModel;

namespace VenueReservation.Api.DTOs.Venues;

public record SearchVenuesRequest(
    DateTime DateTime,
    int DurationInHours,
    int Capacity
);