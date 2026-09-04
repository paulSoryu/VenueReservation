using MediatR;
using VenueReservation.Application.Features.Venues.DTOs;
using VenueReservation.Application.Interfaces;
using VenueReservation.Domain.Models.Venues;
using VenueReservation.Domain.Models.Venues.ValueObjects;
using VenueReservation.Domain.Results;

namespace VenueReservation.Application.Features.Venues.Commands.CreateVenue;

public class CreateVenueCommandHandler(IVenueRepository venueRepository)
    : IRequestHandler<CreateVenueCommand, Result<GetVenueByIdResponse>>
{
    public async Task<Result<GetVenueByIdResponse>> Handle(CreateVenueCommand command, CancellationToken cancellationToken)
    {
        var nameResult = VenueName.Create(command.Name);
        if (nameResult.IsFailure) return Result<GetVenueByIdResponse>.Failure(nameResult.Error);

        var capacityResult = Capacity.Create(command.Capacity);
        if (capacityResult.IsFailure) return Result<GetVenueByIdResponse>.Failure(capacityResult.Error);

        var priceResult = PricePerHour.Create(command.PricePerHour);
        if (priceResult.IsFailure) return Result<GetVenueByIdResponse>.Failure(priceResult.Error);

        var servicesToCreate = command.AvailableServices?
            .Select(s => new Venue.ServiceCreateProps(s.Name, s.Price));

        var venueResult = Venue.Create(
            nameResult.Value,
            capacityResult.Value,
            priceResult.Value,
            servicesToCreate);

        if (venueResult.IsFailure) return Result<GetVenueByIdResponse>.Failure(venueResult.Error);

        var venue = venueResult.Value;

        await venueRepository.AddAsync(venue, cancellationToken);
        await venueRepository.SaveChangesAsync(cancellationToken);

        var response = GetVenueByIdResponse.FromDomain(venue);

        return Result<GetVenueByIdResponse>.Success(response);
    }
}