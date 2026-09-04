using Microsoft.AspNetCore.Mvc;
using VenueReservation.Application.Validation.Errors;
using VenueReservation.Domain.Results;
using VenueReservation.Domain.Results.Errors;

namespace VenueReservation.Api.Controllers;

[ApiController]
public class ApiControllerBase : ControllerBase
{
    protected IActionResult Problem(Result result)
    {
        var error = result?.Error;

        if (error is null)
        {
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Server Error"
            );
        }

        if (error.Type == ErrorType.Validation)
        {
            // Check if the error is of type ValidationError used in MediatR Pipeline and extract the errors dictionary
            var errorsDictionary = error is ValidationError validationError
                ? validationError.Errors
                : new Dictionary<string, string[]> { { "General", [error.Message] } };

            var validationProblem = new ValidationProblemDetails(errorsDictionary)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Error",
                Detail = error.Message
            };

            return BadRequest(validationProblem);
        }

        return error.Type switch
        {
            ErrorType.NotFound => Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Not Found",
                detail: error.Message),

            ErrorType.Conflict => Problem(
                statusCode: StatusCodes.Status409Conflict,
                title: "Business Rule Violation",
                detail: error.Message),

            _ => Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal Server Error",
                detail: error.Message)
        };
    }
}