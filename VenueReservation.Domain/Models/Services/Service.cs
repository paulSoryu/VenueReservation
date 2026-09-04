using VenueReservation.Domain.Models.Services.ValueObjects;
using VenueReservation.Domain.Results;
using VenueReservation.Domain.Results.Errors;

namespace VenueReservation.Domain.Models.Services;

public class Service
{
    public Guid Id { get; private set; }
    public ServiceName Name { get; private set; } = null!;
    public ServicePrice Price { get; private set; } = null!;
    public Guid VenueId { get; private set; }

    private Service() { }

    private Service(Guid id, Guid venueId, ServiceName name, ServicePrice price)
    {
        Id = id;
        VenueId = venueId;
        Name = name;
        Price = price;
    }

    internal static Result<Service> Create(Guid venueId, ServiceName name, ServicePrice price)
    {
        if (venueId == Guid.Empty)
            return Result<Service>.Failure(ServiceErrors.EmptyVenueId);

        if (name is null)
            return Result<Service>.Failure(ServiceErrors.NullName);

        if (price is null)
            return Result<Service>.Failure(ServiceErrors.NullPrice);

        var service = new Service(Guid.NewGuid(), venueId, name, price);

        return Result<Service>.Success(service);
    }

    internal Result Update(ServiceName name, ServicePrice price)
    {
        if (name is null)
            return Result.Failure(ServiceErrors.NullName);

        if (price is null)
            return Result.Failure(ServiceErrors.NullPrice);

        Name = name;
        Price = price;

        return Result.Success();
    }
}