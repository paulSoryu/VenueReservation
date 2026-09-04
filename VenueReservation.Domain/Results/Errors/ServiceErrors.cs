
namespace VenueReservation.Domain.Results.Errors;

public static class ServiceErrors
{
    public static readonly DomainError NameIsEmpty = new(
        "Service.NameIsEmpty",
        "Service name can't be empty.",
        ErrorType.Validation);

    public static DomainError NameInvalidLength(int min, int max) => new(
        "Service.NameInvalidLength",
        $"Service name must be between {min} and {max} characters long.",
        ErrorType.Validation);

    public static DomainError InvalidPrice(decimal min, decimal max) => new(
        "Service.InvalidPrice",
        $"Service price must be between {min} and {max}.",
        ErrorType.Validation);

    public static readonly DomainError EmptyVenueId = new(
        "Service.EmptyVenueId",
        "Service must be associated with a valid Venue ID.",
        ErrorType.Validation);

    public static readonly DomainError NullService = new(
        "Service.Null",
        "Service cannot be null.",
        ErrorType.Validation);

    public static readonly DomainError NullName = new(
        "Service.NullName",
        "Service name cannot be null.",
        ErrorType.Validation);

    public static readonly DomainError NullPrice = new(
        "Service.NullPrice",
        "Service price cannot be null.",
        ErrorType.Validation);

    public static readonly DomainError NotFound = new(
        "Service.NotFound",
        "Service not found.",
        ErrorType.NotFound);
}