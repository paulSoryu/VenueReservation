
using MediatR;
using VenueReservation.Application.Features.Venues.DTOs;
using VenueReservation.Application.Interfaces;
using VenueReservation.Domain.Results;

namespace VenueReservation.Application.Features.Venues.Queries.GetRevenueReport;

public class GetRevenueReportQueryHandler(
    IVenueRepository venueRepository,
    IReservationRepository reservationRepository)
    : IRequestHandler<GetRevenueReportQuery, Result<List<VenueRevenueResponse>>>
{
    public async Task<Result<List<VenueRevenueResponse>>> Handle(
        GetRevenueReportQuery query,
        CancellationToken cancellationToken)
    {
        var venues = await venueRepository.GetAllAsync(cancellationToken);
        var reservations = await reservationRepository.GetReservationsInPeriodAsync(query.FromDate, query.ToDate, cancellationToken);

        var report = new List<VenueRevenueResponse>();

        foreach (var venue in venues)
        {
            var venueReservations = reservations.Where(r => r.VenueId == venue.Id).ToList();

            decimal totalRentRevenue = 0m;
            decimal totalServicesRevenue = 0m;

            foreach (var res in venueReservations)
            {
                var rentCost = res.Period.CalculateVenueRentCost(venue.BasePricePerHour.Value);
                totalRentRevenue += rentCost;

                var servicesCost = res.TotalPrice.Value - rentCost;
                totalServicesRevenue += servicesCost;
            }

            report.Add(new VenueRevenueResponse(
                venue.Id,
                venue.Name.Value,
                totalRentRevenue,
                totalServicesRevenue,
                totalRentRevenue + totalServicesRevenue
            ));
        }

        return Result<List<VenueRevenueResponse>>.Success(report);
    }
}