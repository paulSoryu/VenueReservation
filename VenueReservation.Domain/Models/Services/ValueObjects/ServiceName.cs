
using VenueReservation.Domain.Results;
using VenueReservation.Domain.Results.Errors;

namespace VenueReservation.Domain.Models.Services.ValueObjects;

public record ServiceName
{
    public static int MinLength => 2;
    public static int MaxLength => 75;

    public string Value { get; init; }

    private ServiceName(string value) => Value = value;

    public static Result<ServiceName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<ServiceName>.Failure(ServiceErrors.NameIsEmpty);

        var trimmed = value.Trim();
        if (trimmed.Length < MinLength || trimmed.Length > MaxLength)
            return Result<ServiceName>.Failure(ServiceErrors.NameInvalidLength(MinLength, MaxLength));

        return Result<ServiceName>.Success(new ServiceName(trimmed));
    }

    public static implicit operator string(ServiceName name) => name.Value;
}