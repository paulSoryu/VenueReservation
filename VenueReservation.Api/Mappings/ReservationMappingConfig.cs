using Mapster;
using VenueReservation.Api.DTOs.Reservations;
using VenueReservation.Application.Features.Reservations.Commands.BookVenue;

namespace VenueReservation.Api.Mappings;

public class ReservationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<BookVenueRequest, BookVenueCommand>();
    }
}