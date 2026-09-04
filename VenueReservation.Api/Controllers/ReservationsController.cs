using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VenueReservation.Api.Controllers;
using VenueReservation.Api.DTOs.Reservations;
using VenueReservation.Application.Features.Reservations.Commands.BookVenue;

namespace VenueReservation.API.Controllers;

[Route("api/[controller]")]
public class ReservationsController(ISender mediator) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> BookVenue([FromBody] BookVenueRequest request)
    {
        var command = request.Adapt<BookVenueCommand>();
        var result = await mediator.Send(command);

        if (result.IsFailure)
            return Problem(result);

        return Ok(result.Value);
    }
}