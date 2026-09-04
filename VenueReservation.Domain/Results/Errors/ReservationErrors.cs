
namespace VenueReservation.Domain.Results.Errors;

public static class ReservationErrors
{
    public static readonly DomainError StartTimeInPast = new(
        "Reservation.StartTimeInPast",
        "Reservation possible only in the future (minimum 1 hour before the start).",
        ErrorType.Validation);

    public static DomainError InvalidDuration(int min, int max) => new(
        "Reservation.InvalidDuration",
        $"Booking duration must be between {min} and {max} hours.",
        ErrorType.Validation);

    public static DomainError PriceTooLow(decimal min) => new(
        "Reservation.PriceTooLow",
        $"Final booking cost cannot be less than {min} UAH.",
        ErrorType.Validation);

    public static readonly DomainError NullVenue = new(
        "Reservation.NullVenue",
        "Venue for booking cannot be null.",
        ErrorType.Validation);

    public static readonly DomainError NullPeriod = new(
        "Reservation.NullPeriod",
        "Booking period cannot be null.",
        ErrorType.Validation);

    public static DomainError ServiceNotAvailable(string serviceName, string venueName) => new(
        "Reservation.ServiceNotAvailable",
        $"Service '{serviceName}' is not available for venue '{venueName}'.",
        ErrorType.Conflict);

    public static readonly DomainError SomeServicesNotAvailable = new(
        "Reservation.SomeServicesNotAvailable",
        "One or more selected services are not available for this venue.",
        ErrorType.Conflict);

    public static readonly DomainError InvalidTimeFormat = new(
        "BookingPeriod.InvalidTimeFormat",
        "Time must be exactly at the beginning of an hour (e.g., 14:00, not 14:30).",
        ErrorType.Validation);

    public static readonly DomainError VenueAlreadyBooked = new(
        "Reservation.VenueAlreadyBooked",
        "This venue is already booked for the selected time period.",
        ErrorType.Conflict);

    public static readonly DomainError OutsideWorkingHours = new(
        "BookingPeriod.OutsideWorkingHours",
        "Venues can only be booked between 06:00 and 23:00.",
        ErrorType.Validation);

    public static readonly DomainError MissingTimeZone = new(
        "BookingPeriod.MissingTimeZone",
        "The date and time must include a time zone specifier (e.g., '2026-09-05T09:00:00Z').",
        ErrorType.Validation);
}