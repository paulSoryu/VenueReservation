using VenueReservation.Domain.Results.Errors;

namespace VenueReservation.Application.Validation.Errors;

public record NotFoundError : DomainError
{
    public string EntityName { get; }
    public Guid EntityId { get; }

    public NotFoundError(string entityName, Guid entityId)
        : base("RESOURCE_NOT_FOUND", $"{entityName} with ID '{entityId}' not found.", ErrorType.NotFound)
    {
        EntityName = entityName;
        EntityId = entityId;
    }
}