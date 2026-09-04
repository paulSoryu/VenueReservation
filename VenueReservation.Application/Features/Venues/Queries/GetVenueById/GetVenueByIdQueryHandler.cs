
using MediatR;
using VenueReservation.Application.Features.Venues.DTOs;
using VenueReservation.Application.Interfaces;
using VenueReservation.Domain.Results;
using VenueReservation.Domain.Results.Errors;

namespace VenueReservation.Application.Features.Venues.Queries.GetVenueById;

public class GetVenueByIdQueryHandler(IVenueRepository venueRepository)
    : IRequestHandler<GetVenueByIdQuery, Result<GetVenueByIdResponse>>
{
    public async Task<Result<GetVenueByIdResponse>> Handle(GetVenueByIdQuery query, CancellationToken cancellationToken)
    {
        var venue = await venueRepository.GetByIdAsync(query.Id, cancellationToken);

        if (venue is null)
            return Result<GetVenueByIdResponse>.Failure(VenueErrors.NotFound);

        var response = GetVenueByIdResponse.FromDomain(venue);

        return Result<GetVenueByIdResponse>.Success(response);
    }
}