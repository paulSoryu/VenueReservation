

using VenueReservation.Domain.Results.Errors;

namespace VenueReservation.Domain.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public DomainError Error { get; }

    protected Result(bool isSuccess, DomainError error)
    {
        if (isSuccess && error != DomainError.None || !isSuccess && error == DomainError.None)
            throw new ArgumentException("Wrong error state.", nameof(error));

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, DomainError.None);
    public static Result Failure(DomainError error) => new(false, error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    private Result(TValue? value, bool isSuccess, DomainError error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Unable to get value from a failed result.");

    public static Result<TValue> Success(TValue value) => new(value, true, DomainError.None);
    public static new Result<TValue> Failure(DomainError error) => new(default, false, error);
}

