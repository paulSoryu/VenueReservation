using VenueReservation.Domain.Models.Venues.ValueObjects;
using VenueReservation.Domain.Results;
using VenueReservation.Domain.Results.Errors;
using VenueReservation.Domain.Models.Services;
using VenueReservation.Domain.Models.Services.ValueObjects;

namespace VenueReservation.Domain.Models.Venues;

public class Venue
{
    public Guid Id { get; private set; }
    public VenueName Name { get; private set; } = null!;
    public Capacity Capacity { get; private set; } = null!;
    public PricePerHour BasePricePerHour { get; private set; } = null!;

    private readonly List<Service> _availableServices = new();
    public IReadOnlyCollection<Service> AvailableServices => _availableServices.AsReadOnly();

    public record ServiceCreateProps(string Name, decimal Price);
    public record ServiceUpdateProps(Guid? Id, string Name, decimal Price);

    public bool IsDeleted { get; private set; }

    private Venue() { }

    private Venue(Guid id, VenueName name, Capacity capacity, PricePerHour basePricePerHour)
    {
        Id = id;
        Name = name;
        Capacity = capacity;
        BasePricePerHour = basePricePerHour;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }

    public static Result<Venue> Create(
        VenueName name,
        Capacity capacity,
        PricePerHour basePricePerHour,
        IEnumerable<ServiceCreateProps>? servicesToCreate = null)
    {
        if (name is null) return Result<Venue>.Failure(VenueErrors.NullName);
        if (capacity is null) return Result<Venue>.Failure(VenueErrors.NullCapacity);
        if (basePricePerHour is null) return Result<Venue>.Failure(VenueErrors.NullPrice);

        var venue = new Venue(Guid.NewGuid(), name, capacity, basePricePerHour);

        if (servicesToCreate is not null)
        {
            foreach (var props in servicesToCreate)
            {
                var addResult = venue.CreateAndAddService(props.Name, props.Price);
                if (addResult.IsFailure)
                    return Result<Venue>.Failure(addResult.Error);
            }
        }

        return Result<Venue>.Success(venue);
    }

    public Result Update(
        VenueName name,
        Capacity capacity,
        PricePerHour basePricePerHour,
        IEnumerable<ServiceUpdateProps> servicesProps)
    {
        if (name is null) return Result.Failure(VenueErrors.NullName);
        if (capacity is null) return Result.Failure(VenueErrors.NullCapacity);
        if (basePricePerHour is null) return Result.Failure(VenueErrors.NullPrice);

        Name = name;
        Capacity = capacity;
        BasePricePerHour = basePricePerHour;

        var incomingServiceIds = servicesProps
            .Where(p => p.Id.HasValue)
            .Select(p => p.Id!.Value)
            .ToList();

        _availableServices.RemoveAll(s => !incomingServiceIds.Contains(s.Id));

        foreach (var props in servicesProps)
        {
            if (props.Id.HasValue)
            {
                var existingService = _availableServices.FirstOrDefault(s => s.Id == props.Id.Value);
                if (existingService is null)
                    return Result.Failure(ServiceErrors.NotFound);

                var serviceNameResult = ServiceName.Create(props.Name);
                if (serviceNameResult.IsFailure) return Result.Failure(serviceNameResult.Error);

                var servicePriceResult = ServicePrice.Create(props.Price);
                if (servicePriceResult.IsFailure) return Result.Failure(servicePriceResult.Error);

                var updateServiceResult = existingService.Update(serviceNameResult.Value, servicePriceResult.Value);
                if (updateServiceResult.IsFailure)
                    return Result.Failure(updateServiceResult.Error);
            }
            else
            {
                var addResult = CreateAndAddService(props.Name, props.Price);
                if (addResult.IsFailure)
                    return Result.Failure(addResult.Error);
            }
        }

        return Result.Success();
    }

    public bool CanProvideServices(IEnumerable<Service> services)
    {
        return services.All(s => _availableServices.Any(asv => asv.Id == s.Id));
    }

    private Result CreateAndAddService(string name, decimal price)
    {
        var serviceNameResult = ServiceName.Create(name);
        if (serviceNameResult.IsFailure) 
            return Result.Failure(serviceNameResult.Error);

        var servicePriceResult = ServicePrice.Create(price);
        if (servicePriceResult.IsFailure) 
            return Result.Failure(servicePriceResult.Error);

        if (_availableServices.Any(s => s.Name.Value == name))
            return Result.Failure(VenueErrors.ServiceAlreadyExists);

        var serviceResult = Service.Create(this.Id, serviceNameResult.Value, servicePriceResult.Value);
        if (serviceResult.IsFailure) 
            return Result.Failure(serviceResult.Error);

        _availableServices.Add(serviceResult.Value);
        return Result.Success();
    }
}