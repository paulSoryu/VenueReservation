

using MediatR;
using VenueReservation.Application.Features.Reservations.DTOs;
using VenueReservation.Application.Interfaces;
using VenueReservation.Domain.Models.Reservations;
using VenueReservation.Domain.Models.Reservations.ValueObjects;
using VenueReservation.Domain.Results;
using VenueReservation.Domain.Results.Errors;

namespace VenueReservation.Application.Features.Reservations.Commands.BookVenue;

public class BookVenueCommandHandler(
    IVenueRepository venueRepository,
    IReservationRepository reservationRepository)
    : IRequestHandler<BookVenueCommand, Result<BookVenueResponse>>
{
    public async Task<Result<BookVenueResponse>> Handle(
        BookVenueCommand command,
        CancellationToken cancellationToken)
    {
        var venue = await venueRepository.GetByIdAsync(command.VenueId, cancellationToken);
        if (venue is null)
            return Result<BookVenueResponse>.Failure(VenueErrors.NotFound);

        var period = BookingPeriod.Create(command.StartTime, command.DurationInHours).Value;

        var bookedIds = await reservationRepository.GetBookedVenueIdsAsync(period.StartTime, period.EndTime, cancellationToken);
        if (bookedIds.Contains(venue.Id))
            return Result<BookVenueResponse>.Failure(ReservationErrors.VenueAlreadyBooked);

        var selectedServices = venue.AvailableServices
            .Where(s => command.SelectedServiceIds.Contains(s.Id))
            .ToList();

        var reservationResult = Reservation.Create(venue, period, selectedServices);
        if (reservationResult.IsFailure)
            return Result<BookVenueResponse>.Failure(reservationResult.Error);

        var reservation = reservationResult.Value;

        await reservationRepository.AddAsync(reservation, cancellationToken);
        await venueRepository.SaveChangesAsync(cancellationToken);

        var response = new BookVenueResponse(
            reservation.Id,
            venue.Name.Value,                
            reservation.TotalPrice.Value     
        );

        return Result<BookVenueResponse>.Success(response);
    }
}