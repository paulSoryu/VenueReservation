using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VenueReservation.Api.Controllers;
using VenueReservation.Api.DTOs.Venues;
using VenueReservation.Application.Features.Venues.Commands.CreateVenue;
using VenueReservation.Application.Features.Venues.Commands.DeleteVenue;
using VenueReservation.Application.Features.Venues.Commands.UpdateVenue;
using VenueReservation.Application.Features.Venues.Queries.GetVenueById;
using VenueReservation.Application.Features.Venues.Queries.SearchVenues;

namespace VenueReservation.API.Controllers;

[Route("api/[controller]")]
public class VenuesController(ISender mediator) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateVenue([FromBody] CreateVenueRequest request)
    {
        var command = request.Adapt<CreateVenueCommand>();
        var result = await mediator.Send(command);

        if (result.IsFailure)
            return Problem(result);

        var venueDto = result.Value;

        return CreatedAtRoute(
            "GetVenueById",
            new { id = venueDto.Id },
            venueDto                 
        );
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateVenue(Guid id, [FromBody] UpdateVenueRequest request)
    {
        var command = (request, id).Adapt<UpdateVenueCommand>();
        var result = await mediator.Send(command);

        if (result.IsFailure)
            return Problem(result);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteVenue(Guid id, [FromQuery] bool isSoftDelete = true)
    {
        var command = new DeleteVenueCommand(id, isSoftDelete);
        var result = await mediator.Send(command);

        if (result.IsFailure)
            return Problem(result);

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchVenues([FromQuery] SearchVenuesRequest request)
    {
        var query = request.Adapt<SearchVenuesQuery>();

        var result = await mediator.Send(query);

        if (result.IsFailure)
            return Problem(result);

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}", Name = "GetVenueById")]
    public async Task<IActionResult> GetVenueById(Guid id)
    {
        var query = new GetVenueByIdQuery(id);
        var result = await mediator.Send(query);

        if (result.IsFailure)
            return Problem(result);

        return Ok(result.Value);
    }
}