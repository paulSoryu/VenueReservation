
using MediatR;
using VenueReservation.Application.Features.Venues.DTOs;
using VenueReservation.Application.Interfaces;
using VenueReservation.Domain.Models.Reservations.ValueObjects;
using VenueReservation.Domain.Results;

namespace VenueReservation.Application.Features.Venues.Queries.GetUtilizationReport;

public class GetUtilizationReportQueryHandler(
    IVenueRepository venueRepository,
    IReservationRepository reservationRepository)
    : IRequestHandler<GetUtilizationReportQuery, Result<List<VenueUtilizationResponse>>>
{
    public async Task<Result<List<VenueUtilizationResponse>>> Handle(
        GetUtilizationReportQuery query,
        CancellationToken cancellationToken)
    {
        int totalDays = query.ToDate.DayNumber - query.FromDate.DayNumber + 1;
        if (totalDays <= 0) totalDays = 1;

        double totalAvailableWorkingHours = totalDays * BookingPeriod.TotalWorkingHoursPerDay;

        var venues = await venueRepository.GetAllAsync(cancellationToken);
        var reservations = await reservationRepository.GetReservationsInPeriodAsync(query.FromDate, query.ToDate, cancellationToken);

        var report = new List<VenueUtilizationResponse>();

        foreach (var venue in venues)
        {
            int bookedHours = reservations
                .Where(r => r.VenueId == venue.Id)
                .Sum(r => r.Period.DurationInHours);

            double utilizationPercentage = totalAvailableWorkingHours > 0
                ? Math.Round((bookedHours / totalAvailableWorkingHours) * 100, 2)
                : 0;

            report.Add(new VenueUtilizationResponse(
                venue.Id,
                venue.Name.Value,
                bookedHours,
                utilizationPercentage
            ));
        }

        return Result<List<VenueUtilizationResponse>>.Success(report);
    }
}