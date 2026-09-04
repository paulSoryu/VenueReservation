
using VenueReservation.Domain.Results;
using VenueReservation.Domain.Results.Errors;

namespace VenueReservation.Domain.Models.Venues.ValueObjects;

public record PricePerHour
{
    public static decimal MinValue => 100.00m;
    public static decimal MaxValue => 100000.00m;

    public decimal Value { get; init; }

    private PricePerHour(decimal value) => Value = value;

    public static Result<PricePerHour> Create(decimal value)
    {
        if (value < MinValue || value > MaxValue)
        {
            return Result<PricePerHour>.Failure(VenueErrors.InvalidPrice(MinValue, MaxValue));
        }

        return Result<PricePerHour>.Success(new PricePerHour(value));
    }

    public static implicit operator decimal(PricePerHour price) => price.Value;
}