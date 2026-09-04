
using MediatR;
using VenueReservation.Application.Features.Venues.DTOs;
using VenueReservation.Domain.Results;

namespace VenueReservation.Application.Features.Venues.Queries.GetRevenueReport;

public record GetRevenueReportQuery(DateOnly FromDate, DateOnly ToDate)
    : IRequest<Result<List<VenueRevenueResponse>>>;