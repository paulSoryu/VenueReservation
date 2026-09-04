using VenueReservation.Domain.Results;
using VenueReservation.Domain.Results.Errors;

namespace VenueReservation.Domain.Models.Venues.ValueObjects;

public record VenueName
{
    public static int MinLength => 3;
    public static int MaxLength => 100;

    public string Value { get; init; }

    private VenueName(string value) => Value = value;

    public static Result<VenueName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<VenueName>.Failure(VenueErrors.NameIsEmpty);

        var trimmed = value.Trim();
        if (trimmed.Length < MinLength || trimmed.Length > MaxLength)
            return Result<VenueName>.Failure(VenueErrors.NameInvalidLength(MinLength, MaxLength));

        return Result<VenueName>.Success(new VenueName(trimmed));
    }

    public static implicit operator string(VenueName name) => name.Value;
}