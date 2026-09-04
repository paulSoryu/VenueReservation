using FluentValidation;
using VenueReservation.Domain.Models.Reservations.ValueObjects;
using VenueReservation.Domain.Models.Venues.ValueObjects;
using VenueReservation.Domain.Results.Errors;

namespace VenueReservation.Application.Features.Venues.Queries.SearchVenues;

public class SearchVenuesQueryValidator : AbstractValidator<SearchVenuesQuery>
{
    public SearchVenuesQueryValidator()
    {
        RuleFor(x => x.DateTime)
            .NotEmpty()
            .WithMessage("Date and time are required.");

        RuleFor(x => x.DurationInHours)
            .NotEmpty()
            .WithMessage("Duration in hours is required.");

        RuleFor(x => x.Capacity)
            .NotEmpty()
            .WithMessage("Capacity is required.");

        RuleFor(x => x)
            .Custom((query, context) =>
            {
                if (query.DateTime != default && query.DurationInHours > 0)
                {
                    if (query.DateTime.Kind != DateTimeKind.Utc)
                    {
                        context.AddFailure(nameof(SearchVenuesQuery.DateTime), ReservationErrors.MissingTimeZone.Message);
                        return;
                    }

                    var result = BookingPeriod.Create(query.DateTime, query.DurationInHours);
                    if (result.IsFailure)
                    {
                        context.AddFailure(nameof(SearchVenuesQuery.DateTime), result.Error.Message);
                    }
                }
            });

        RuleFor(x => x.Capacity)
            .Custom((capacity, context) =>
            {
                if (capacity > 0)
                {
                    var result = Capacity.Create(capacity);
                    if (result.IsFailure)
                    {
                        context.AddFailure(nameof(SearchVenuesQuery.Capacity), result.Error.Message);
                    }
                }
            });
    }
}