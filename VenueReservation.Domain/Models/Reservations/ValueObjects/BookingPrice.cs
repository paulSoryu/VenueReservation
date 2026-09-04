
using VenueReservation.Domain.Results;
using VenueReservation.Domain.Results.Errors;

namespace VenueReservation.Domain.Models.Reservations.ValueObjects;

public record BookingPrice
{
    public static decimal MinValue => 100.00m;

    public decimal Value { get; init; }

    private BookingPrice(decimal value) => Value = value;

    public static Result<BookingPrice> Create(decimal value)
    {
        if (value < MinValue)
            return Result<BookingPrice>.Failure(ReservationErrors.PriceTooLow(MinValue));

        return Result<BookingPrice>.Success(new BookingPrice(value));
    }

    public static implicit operator decimal(BookingPrice price) => price.Value;
}