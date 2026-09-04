namespace VenueReservation.Application.Features.Venues.DTOs;

public record VenueRevenueResponse(
    Guid VenueId,
    string VenueName,
    decimal TotalRentRevenue,       
    decimal TotalServicesRevenue,   
    decimal CombinedTotalRevenue    
);