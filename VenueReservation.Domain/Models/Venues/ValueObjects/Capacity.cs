using VenueReservation.Domain.Results;
using VenueReservation.Domain.Results.Errors;

namespace VenueReservation.Domain.Models.Venues.ValueObjects;

public record Capacity
{
    public static int MinValue => 10;
    public static int MaxValue => 500;

    public int Value { get; init; }

    private Capacity(int value) => Value = value;

    public static Result<Capacity> Create(int value)
    {
        if (value < MinValue || value > MaxValue)
            return Result<Capacity>.Failure(VenueErrors.InvalidCapacity(MinValue, MaxValue));

        return Result<Capacity>.Success(new Capacity(value));
    }

    public static implicit operator int(Capacity capacity) => capacity.Value;
}