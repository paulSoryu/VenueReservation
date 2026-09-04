
using FluentValidation;

namespace VenueReservation.Application.Features.Venues.Queries.GetUtilizationReport;

public class GetUtilizationReportQueryValidator : AbstractValidator<GetUtilizationReportQuery>
{
    public GetUtilizationReportQueryValidator()
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
                    // Проверяем, что дата "До" не идет раньше даты "От"
                    if (query.ToDate < query.FromDate)
                    {
                        context.AddFailure(nameof(GetUtilizationReportQuery.ToDate),
                            "The end date (ToDate) cannot be earlier than the start date (FromDate).");
                    }
                }
            });
    }
}