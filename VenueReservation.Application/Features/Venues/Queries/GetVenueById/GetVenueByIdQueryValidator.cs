using FluentValidation;

namespace VenueReservation.Application.Features.Venues.Queries.GetVenueById;

public class GetVenueByIdQueryValidator : AbstractValidator<GetVenueByIdQuery>
{
    public GetVenueByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Venue ID cannot be empty.");
    }
}