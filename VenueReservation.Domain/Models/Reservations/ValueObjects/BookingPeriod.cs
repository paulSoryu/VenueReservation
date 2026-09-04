
using VenueReservation.Domain.Results;
using VenueReservation.Domain.Results.Errors;

namespace VenueReservation.Domain.Models.Reservations.ValueObjects;

public record BookingPeriod
{
    public static int MinDuration => 1;
    public static int MaxDuration => 12;

    public static int WorkingHoursStart => 6;  // 06:00
    public static int WorkingHoursEnd => 23;   // 23:00
    public static int TotalWorkingHoursPerDay => WorkingHoursEnd - WorkingHoursStart;
    public DateTime StartTime { get; init; }
    public int DurationInHours { get; init; }
    public DateTime EndTime => StartTime.AddHours(DurationInHours);

    private BookingPeriod(DateTime startTime, int durationInHours)
    {
        StartTime = startTime;
        DurationInHours = durationInHours;
    }

    public static Result<BookingPeriod> Create(DateTime startTime, int durationInHours)
    {
        if (startTime.Minute != 0 || startTime.Second != 0 || startTime.Millisecond != 0)
            return Result<BookingPeriod>.Failure(ReservationErrors.InvalidTimeFormat);

        if (startTime <= DateTime.UtcNow.AddMinutes(50))
            return Result<BookingPeriod>.Failure(ReservationErrors.StartTimeInPast);

        if (durationInHours < MinDuration || durationInHours > MaxDuration)
            return Result<BookingPeriod>.Failure(ReservationErrors.InvalidDuration(MinDuration, MaxDuration));

        var endTime = startTime.AddHours(durationInHours);

        if (startTime.Hour < WorkingHoursStart ||
            endTime.Hour > WorkingHoursEnd ||
            (endTime.Hour == WorkingHoursEnd && endTime.Minute > 0))
        {
            return Result<BookingPeriod>.Failure(ReservationErrors.OutsideWorkingHours);
        }

        return Result<BookingPeriod>.Success(new BookingPeriod(startTime, durationInHours));
    }

    public decimal CalculateVenueRentCost(decimal basePricePerHour)
    {
        decimal totalRentCost = 0m;

        for (int i = 0; i < DurationInHours; i++)
        {
            DateTime currentHourStart = StartTime.AddHours(i);
            decimal coefficient = GetHourlyCoefficient(currentHourStart);

            totalRentCost += basePricePerHour * coefficient;
        }

        return totalRentCost;
    }

    private static decimal GetHourlyCoefficient(DateTime hourStart)
    {
        int hour = hourStart.Hour;

        if (hour >= 12 && hour < 14) return 1.15m;

        if (hour >= 6 && hour < 9) return 0.90m;

        if (hour >= 18 && hour < 23) return 0.80m;

        return 1.00m;
    }
}