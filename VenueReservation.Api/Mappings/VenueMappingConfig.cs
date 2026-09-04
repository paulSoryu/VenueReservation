using Mapster;
using VenueReservation.Api.DTOs.Venues;
using VenueReservation.Application.Features.Venues.Commands.CreateVenue;
using VenueReservation.Application.Features.Venues.Commands.UpdateVenue;

namespace VenueReservation.Api.Mappings;

public class VenueMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateVenueRequest, CreateVenueCommand>()
            .Map(dest => dest.AvailableServices, src => src.AvailableServices
                .Select(s => new CreateServiceCommandDto(s.Name, s.Price)).ToList());

        config.NewConfig<(UpdateVenueRequest Request, Guid Id), UpdateVenueCommand>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Request.Name)
            .Map(dest => dest.Capacity, src => src.Request.Capacity)
            .Map(dest => dest.BasePricePerHour, src => src.Request.BasePricePerHour)
            .Map(dest => dest.AvailableServices, src => src.Request.AvailableServices
                .Select(s => new UpdateServiceCommandDto(s.Id, s.Name, s.Price)).ToList());
    }
}