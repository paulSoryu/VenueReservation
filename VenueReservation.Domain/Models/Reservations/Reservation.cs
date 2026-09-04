using VenueReservation.Domain.Models.Reservations.ValueObjects;
using VenueReservation.Domain.Models.Services;
using VenueReservation.Domain.Models.Venues;
using VenueReservation.Domain.Results;
using VenueReservation.Domain.Results.Errors;

namespace VenueReservation.Domain.Models.Reservations;

public class Reservation
{
    public Guid Id { get; private set; }
    public BookingPeriod Period { get; private set; } = null!;
    public BookingPrice TotalPrice { get; private set; } = null!;
    public Guid VenueId { get; private set; } 

    private readonly List<Service> _selectedServices = new();
    public IReadOnlyCollection<Service> SelectedServices => _selectedServices.AsReadOnly();

    private Reservation() { }

    private Reservation(Guid id, Guid venueId, BookingPeriod period, BookingPrice totalPrice, List<Service> selectedServices)
    {
        Id = id;
        VenueId = venueId;
        Period = period;
        TotalPrice = totalPrice;
        _selectedServices = selectedServices;
    }

    public static Result<Reservation> Create(Venue venue, BookingPeriod period, List<Service> selectedServices)
    {
        if (venue is null) return Result<Reservation>.Failure(ReservationErrors.NullVenue);
        if (period is null) return Result<Reservation>.Failure(ReservationErrors.NullPeriod);

        selectedServices ??= new List<Service>();

        if (!venue.CanProvideServices(selectedServices))
            return Result<Reservation>.Failure(ReservationErrors.SomeServicesNotAvailable);

        decimal totalRentPrice = period.CalculateVenueRentCost(venue.BasePricePerHour.Value);

        decimal servicesPrice = selectedServices.Sum(s => s.Price.Value);
        
        var finalCalculatedAmount = totalRentPrice + servicesPrice;

        var priceResult = BookingPrice.Create(finalCalculatedAmount);
        if (priceResult.IsFailure) 
            return Result<Reservation>.Failure(priceResult.Error);

        return Result<Reservation>.Success(new Reservation(
            Guid.NewGuid(),
            venue.Id,
            period,
            priceResult.Value,
            [.. selectedServices]
        ));
    }
}