
namespace VenueReservation.Domain.Results.Errors;

public static class VenueErrors
{
    public static readonly DomainError NameIsEmpty = new(
        "VenueName.Empty",
        "Venue name cannot be empty.",
        ErrorType.Validation);

    public static DomainError NameInvalidLength(int min, int max) => new(
        "VenueName.InvalidLength",
        $"Venue name must be between {min} and {max} characters.",
        ErrorType.Validation);

    public static DomainError InvalidCapacity(int min, int max) => new(
        "Venue.InvalidCapacity",
        $"Capacity of venue must be between {min} and {max} people.",
        ErrorType.Validation);

    public static DomainError InvalidPrice(decimal min, decimal max) => new(
        "Venue.InvalidPricePerHour",
        $"Price per hour must be between {min} and {max} UAH/hour.",
        ErrorType.Validation);

    public static readonly DomainError NotFound = new(
        "Venue.NotFound",
        "Venue not found.",
        ErrorType.NotFound);

    public static readonly DomainError NullName = new(
        "Venue.NullName",
        "Object name cannot be null.",
        ErrorType.Validation);

    public static readonly DomainError NullCapacity = new(
        "Venue.NullCapacity",
        "Object capacity cannot be null.",
        ErrorType.Validation);

    public static readonly DomainError NullPrice = new(
        "Venue.NullPrice",
        "Object price cannot be null.",
        ErrorType.Validation);

    public static readonly DomainError ServiceAlreadyExists = new(
        "Venue.ServiceAlreadyExists",
        "This service is already added to the venue.",
        ErrorType.Conflict);

    public static readonly DomainError HasActiveBookings = new(
        "Venue.HasActiveBookings",
        "Cannot delete the venue because it has active reservations.",
        ErrorType.Conflict);
}