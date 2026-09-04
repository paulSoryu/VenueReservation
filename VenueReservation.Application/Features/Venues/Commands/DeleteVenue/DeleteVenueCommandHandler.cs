using MediatR;
using VenueReservation.Application.Interfaces;
using VenueReservation.Domain.Results;
using VenueReservation.Domain.Results.Errors;

namespace VenueReservation.Application.Features.Venues.Commands.DeleteVenue;

public class DeleteVenueCommandHandler(IVenueRepository venueRepository)
    : IRequestHandler<DeleteVenueCommand, Result>
{
    public async Task<Result> Handle(DeleteVenueCommand command, CancellationToken cancellationToken)
    {
        var venue = await venueRepository.GetByIdNoFiltersAsync(command.Id, cancellationToken);
        if (venue is null)
            return Result.Failure(VenueErrors.NotFound);

        var hasFutureReservations = await venueRepository.HasAnyReservationsAsync(command.Id, cancellationToken);

        if (command.IsSoftDelete)
            venueRepository.SoftDelete(venue);
        else
        {
            if (hasFutureReservations)
                return Result.Failure(VenueErrors.HasActiveBookings); 

            venueRepository.HardDelete(venue);
        }

        await venueRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}