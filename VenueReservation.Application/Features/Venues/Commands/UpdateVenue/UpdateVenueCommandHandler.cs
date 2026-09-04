
using MediatR;
using VenueReservation.Application.Interfaces;
using VenueReservation.Domain.Models.Venues;
using VenueReservation.Domain.Models.Venues.ValueObjects;
using VenueReservation.Domain.Results;
using VenueReservation.Domain.Results.Errors;

namespace VenueReservation.Application.Features.Venues.Commands.UpdateVenue;

public class UpdateVenueCommandHandler(IVenueRepository venueRepository)
    : IRequestHandler<UpdateVenueCommand, Result>
{
    public async Task<Result> Handle(UpdateVenueCommand command, CancellationToken cancellationToken)
    {
        var venue = await venueRepository.GetByIdAsync(command.Id, cancellationToken);

        if (venue is null)
            return Result.Failure(VenueErrors.NotFound);

        var nameResult = VenueName.Create(command.Name);
        if (nameResult.IsFailure) return Result.Failure(nameResult.Error);

        var capacityResult = Capacity.Create(command.Capacity);
        if (capacityResult.IsFailure) return Result.Failure(capacityResult.Error);

        var priceResult = PricePerHour.Create(command.BasePricePerHour);
        if (priceResult.IsFailure) return Result.Failure(priceResult.Error);

        var servicesProps = command.AvailableServices
            .Select(s => new Venue.ServiceUpdateProps(s.Id, s.Name, s.Price));

        var updateResult = venue.Update(
            nameResult.Value,
            capacityResult.Value,
            priceResult.Value,
            servicesProps);

        if (updateResult.IsFailure)
            return Result.Failure(updateResult.Error);

        await venueRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
