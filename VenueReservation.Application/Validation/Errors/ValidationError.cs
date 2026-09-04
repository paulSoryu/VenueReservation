using VenueReservation.Domain.Results.Errors;

namespace VenueReservation.Application.Validation.Errors;

public record ValidationError : DomainError
{
    public Dictionary<string, string[]> Errors { get; } = new();

    public ValidationError(string message)
        : base("BUSINESS_VALIDATION_ERROR", message, ErrorType.Validation)
    {
    }

    public ValidationError(string message, Dictionary<string, string[]> errors)
        : base("BUSINESS_VALIDATION_ERROR", message, ErrorType.Validation)
    {
        Errors = errors;
    }
}