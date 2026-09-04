
using MediatR;
using VenueReservation.Application.Features.Venues.DTOs;
using VenueReservation.Domain.Results;

namespace VenueReservation.Application.Features.Venues.Queries.GetUtilizationReport;

public record GetUtilizationReportQuery(DateOnly FromDate, DateOnly ToDate)
    : IRequest<Result<List<VenueUtilizationResponse>>>;