using VenueReservation.Domain.Models.Reservations;
using VenueReservation.Domain.Models.Services;

namespace VenueReservation.Domain.Models;

// This class represents the many-to-many relationship between Reservation and AdditionalService.
// We can add additional properties here if needed, such as quantity or special instructions for the service.

// IMPORTANT: Currently Reservation and AdditionalService navigational properties are referencing each other directly, but if you add new properties to this join entity, you'll need to adjust the configurations to account for those properties.
// This entity currently exists only to have direct control over the many-to-many relationship between Reservation and AdditionalService, and to allow for potential future expansion.
public class ReservationService
{
    public Guid ReservationId { get; set; }
    public Reservation Reservation { get; set; } = null!;

    public Guid ServiceId { get; set; }
    public Service Service { get; set; } = null!;
}