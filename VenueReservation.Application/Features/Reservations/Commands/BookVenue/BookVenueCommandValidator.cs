
using FluentValidation;
using VenueReservation.Domain.Models.Reservations.ValueObjects;
using VenueReservation.Domain.Results.Errors;

namespace VenueReservation.Application.Features.Reservations.Commands.BookVenue;

public class BookVenueCommandValidator : AbstractValidator<BookVenueCommand>
{
    public BookVenueCommandValidator()
    {
        RuleFor(x => x.VenueId)
            .NotEmpty()
            .WithMessage("Venue ID is required.");

        RuleFor(x => x.StartTime)
            .NotEmpty()
            .WithMessage("Reservation start time is required.");

        RuleFor(x => x.DurationInHours)
            .NotEmpty()
            .WithMessage("Reservation duration is required.");

        RuleFor(x => x.SelectedServiceIds)
            .NotNull()
            .WithMessage("Selected services list cannot be null.");

        RuleFor(x => x)
            .Custom((command, context) =>
            {
                if (command.StartTime != default && command.DurationInHours > 0)
                {
                    if (command.StartTime.Kind != DateTimeKind.Utc)
                    {
                        context.AddFailure(nameof(BookVenueCommand.StartTime), ReservationErrors.MissingTimeZone.Message);
                        return;
                    }

                    var result = BookingPeriod.Create(command.StartTime, command.DurationInHours);
                    if (result.IsFailure)
                    {
                        context.AddFailure(nameof(BookVenueCommand.StartTime), result.Error.Message);
                    }
                }
            });
    }
}