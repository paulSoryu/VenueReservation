
using FluentValidation;

namespace VenueReservation.Application.Features.Venues.Commands.DeleteVenue;

public class DeleteVenueCommandValidator : AbstractValidator<DeleteVenueCommand>
{
    public DeleteVenueCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Venue ID cannot be empty.");
    }
}