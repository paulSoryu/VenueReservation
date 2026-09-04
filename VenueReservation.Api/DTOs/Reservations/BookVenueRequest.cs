using System.ComponentModel;

namespace VenueReservation.Api.DTOs.Reservations;

public record BookVenueRequest(
    Guid VenueId,
    DateTime StartTime,
    int DurationInHours,
    List<Guid> SelectedServiceIds
);