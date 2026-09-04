using Microsoft.EntityFrameworkCore;
using VenueReservation.Domain.Models.Venues;
using VenueReservation.Domain.Models.Venues.ValueObjects;

namespace VenueReservation.Infrastructure.Data.Seeders;

public static class DatabaseSeeder
{
    public static readonly Guid VenueAId = Guid.Parse("a1111111-1111-1111-1111-111111111111");
    public static readonly Guid VenueBId = Guid.Parse("b2222222-2222-2222-2222-222222222222");
    public static readonly Guid VenueCId = Guid.Parse("c3333333-3333-3333-3333-333333333333");

    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Venues.AnyAsync())
            return;

        var defaultServicesProps = new List<Venue.ServiceCreateProps>
        {
            new("Проєктор", 500m),
            new("Wi-Fi", 300m),
            new("Звук", 700m)
        };

        var venueAResult = Venue.Create(VenueName.Create("Зал А").Value, Capacity.Create(50).Value, PricePerHour.Create(2000m).Value, defaultServicesProps);
        var venueBResult = Venue.Create(VenueName.Create("Зал B").Value, Capacity.Create(100).Value, PricePerHour.Create(3500m).Value, defaultServicesProps);
        var venueCResult = Venue.Create(VenueName.Create("Зал C").Value, Capacity.Create(30).Value, PricePerHour.Create(1500m).Value, defaultServicesProps);

        var venueA = venueAResult.Value;
        var venueB = venueBResult.Value;
        var venueC = venueCResult.Value;

        SetPrivateId(venueA, VenueAId);
        SetPrivateId(venueB, VenueBId);
        SetPrivateId(venueC, VenueCId);

        FixNestedServicesIds(venueA);
        FixNestedServicesIds(venueB);
        FixNestedServicesIds(venueC);

        await context.Venues.AddRangeAsync(venueA, venueB, venueC);

        await context.SaveChangesAsync();
    }

    private static void FixNestedServicesIds(Venue venue)
    {
        var venuePrefix = venue.Id.ToString()[..8];
        int counter = 1;

        foreach (var service in venue.AvailableServices)
        {
            var serviceGuid = Guid.Parse($"{venuePrefix}-0000-0000-0000-00000000000{counter}");

            var serviceType = service.GetType();
            serviceType.GetProperty("Id")?.SetValue(service, serviceGuid);

            counter++;
        }
    }

    private static void SetPrivateId<T>(T entity, Guid id) where T : class
    {
        typeof(T).GetProperty("Id")?.SetValue(entity, id, null);
    }
}