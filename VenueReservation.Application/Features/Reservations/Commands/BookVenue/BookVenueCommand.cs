using MediatR;
using VenueReservation.Application.Features.Reservations.DTOs;
using VenueReservation.Domain.Results;

namespace VenueReservation.Application.Features.Reservations.Commands.BookVenue;

public record BookVenueCommand(
    Guid VenueId,
    DateTime StartTime,
    int DurationInHours,
    List<Guid> SelectedServiceIds
) : IRequest<Result<BookVenueResponse>>;