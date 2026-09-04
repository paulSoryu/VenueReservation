using MediatR;
using Microsoft.AspNetCore.Mvc;
using VenueReservation.Application.Features.Venues.Queries.GetRevenueReport;
using VenueReservation.Application.Features.Venues.Queries.GetUtilizationReport;

namespace VenueReservation.Api.Controllers;

[Route("api/[controller]")]
public class AnalyticsController(ISender mediator) : ApiControllerBase
{
    [HttpGet("revenue")]
    public async Task<IActionResult> GetRevenueReport([FromQuery] DateOnly from, [FromQuery] DateOnly to)
    {
        var query = new GetRevenueReportQuery(from, to);
        var result = await mediator.Send(query);

        if (result.IsFailure)
            return Problem(result);

        return Ok(result.Value);
    }

    [HttpGet("utilization")]
    public async Task<IActionResult> GetUtilizationReport([FromQuery] DateOnly from, [FromQuery] DateOnly to)
    {
        var query = new GetUtilizationReportQuery(from, to);
        var result = await mediator.Send(query);

        if (result.IsFailure)
            return Problem(result);

        return Ok(result.Value);
    }
}