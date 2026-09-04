namespace VenueReservation.Application.Features.Reservations.DTOs;

public record BookVenueResponse(
    Guid ReservationId,
    string RoomName,
    decimal CalculatedTotalPrice
);