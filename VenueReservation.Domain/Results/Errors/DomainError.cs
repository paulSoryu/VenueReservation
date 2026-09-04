
namespace VenueReservation.Domain.Results.Errors;

public enum ErrorType
{
    Failure,     
    Validation,  
    NotFound,    
    Conflict     
}

public record DomainError(string Code, string Message, ErrorType Type)
{
    public static readonly DomainError None = new(string.Empty, string.Empty, ErrorType.Failure);
}