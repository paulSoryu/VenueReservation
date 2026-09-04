using FluentValidation;

namespace VenueReservation.Application.Features.Venues.Queries.GetRevenueReport;

public class GetRevenueReportQueryValidator : AbstractValidator<GetRevenueReportQuery>
{
    public GetRevenueReportQueryValidator()
    {
        RuleFor(x => x.FromDate)
            .NotEmpty()
            .WithMessage("Start date (FromDate) is required.");

        RuleFor(x => x.ToDate)
            .NotEmpty()
            .WithMessage("End date (ToDate) is required.");

        RuleFor(x => x)
            .Custom((query, context) =>
            {
                if (query.FromDate != default && query.ToDate != default)
                {
                    if (query.ToDate < query.FromDate)
                    {
                        context.AddFailure(nameof(GetRevenueReportQuery.ToDate),
                            "The end date (ToDate) cannot be earlier than the start date (FromDate).");
                    }
                }
            });
    }
}