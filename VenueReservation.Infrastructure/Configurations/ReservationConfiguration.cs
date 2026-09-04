using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VenueReservation.Domain.Models;
using VenueReservation.Domain.Models.Reservations;
using VenueReservation.Domain.Models.Reservations.ValueObjects;
using VenueReservation.Domain.Models.Venues;

namespace VenueReservation.Infrastructure.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");

        builder.HasKey(r => r.Id);

        builder.OwnsOne(r => r.Period, periodBuilder =>
        {
            periodBuilder.Property(p => p.StartTime)
                .HasColumnName("StartTime")
                .IsRequired();

            periodBuilder.Property(p => p.DurationInHours)
                .HasColumnName("DurationInHours")
                .IsRequired();
        });

        builder.Property(r => r.TotalPrice)
            .HasConversion(
                priceObject => priceObject.Value,
                decimalValue => BookingPrice.Create(decimalValue).Value
            )
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.HasOne<Venue>()
            .WithMany()
            .HasForeignKey(r => r.VenueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.SelectedServices)
               .WithMany()
               .UsingEntity<ReservationService>(
                    l => l.HasOne(rs => rs.Service)
                          .WithMany()
                          .HasForeignKey(rs => rs.ServiceId)
                          .OnDelete(DeleteBehavior.Cascade),

                    r => r.HasOne(rs => rs.Reservation)
                          .WithMany()
                          .HasForeignKey(rs => rs.ReservationId)
                          .OnDelete(DeleteBehavior.Cascade),

                    j =>
                    {
                        j.ToTable("ReservationServices");
                        j.HasKey(rs => new { rs.ReservationId, rs.ServiceId });
                    }
                );

        builder.Navigation(r => r.SelectedServices)
            .Metadata.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}