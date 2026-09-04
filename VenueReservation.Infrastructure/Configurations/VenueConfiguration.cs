using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VenueReservation.Domain.Models.Venues.ValueObjects;
using VenueReservation.Domain.Models.Venues;

namespace VenueReservation.Infrastructure.Configurations;

public class VenueConfiguration : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> builder)
    {
        builder.ToTable("Venues");
        builder.HasKey(v => v.Id);

        builder.HasQueryFilter(v => !v.IsDeleted);

        builder.Property(v => v.Name)
               .HasConversion(n => n.Value, s => VenueName.Create(s).Value)
               .IsRequired()
               .HasMaxLength(VenueName.MaxLength);

        builder.Property(v => v.Capacity)
               .HasConversion(c => c.Value, i => Capacity.Create(i).Value)
               .IsRequired();

        builder.Property(v => v.BasePricePerHour)
               .HasConversion(p => p.Value, d => PricePerHour.Create(d).Value)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.HasMany(v => v.AvailableServices)
               .WithOne()
               .HasForeignKey(s => s.VenueId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(v => v.AvailableServices)
            .Metadata.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}