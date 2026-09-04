
using VenueReservation.Domain.Results;
using VenueReservation.Domain.Results.Errors;

namespace VenueReservation.Domain.Models.Services.ValueObjects;

public record ServicePrice
{
    public static decimal MinValue => 0.00m;
    public static decimal MaxValue => 15000.00m;

    public decimal Value { get; init; }

    private ServicePrice(decimal value) => Value = value;

    public static Result<ServicePrice> Create(decimal value)
    {
        if (value < MinValue || value > MaxValue)
            return Result<ServicePrice>.Failure(ServiceErrors.InvalidPrice(MinValue, MaxValue));

        return Result<ServicePrice>.Success(new ServicePrice(value));
    }

    public static implicit operator decimal(ServicePrice price) => price.Value;
}