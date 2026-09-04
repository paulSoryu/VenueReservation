
using FluentValidation;
using VenueReservation.Domain.Models.Services.ValueObjects;
using VenueReservation.Domain.Models.Venues.ValueObjects;

namespace VenueReservation.Application.Features.Venues.Commands.UpdateVenue;


public class UpdateVenueCommandValidator : AbstractValidator<UpdateVenueCommand>
{
    public UpdateVenueCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Venue ID cannot be empty.");

        RuleFor(x => x.Name)
            .Custom((name, context) =>
            {
                var result = VenueName.Create(name);
                if (result.IsFailure)
                {
                    context.AddFailure(nameof(UpdateVenueCommand.Name), result.Error.Message);
                }
            });

        RuleFor(x => x.Capacity)
            .Custom((capacity, context) =>
            {
                var result = Capacity.Create(capacity);
                if (result.IsFailure)
                {
                    context.AddFailure(nameof(UpdateVenueCommand.Capacity), result.Error.Message);
                }
            });

        RuleFor(x => x.BasePricePerHour)
            .Custom((price, context) =>
            {
                var result = PricePerHour.Create(price);
                if (result.IsFailure)
                {
                    context.AddFailure(nameof(UpdateVenueCommand.BasePricePerHour), result.Error.Message);
                }
            });

        RuleForEach(x => x.AvailableServices)
            .SetValidator(new UpdateServiceCommandDtoValidator());
    }
}

public class UpdateServiceCommandDtoValidator : AbstractValidator<UpdateServiceCommandDto>
{
    public UpdateServiceCommandDtoValidator()
    {
        RuleFor(x => x.Name)
            .Custom((name, context) =>
            {
                var result = ServiceName.Create(name);
                if (result.IsFailure)
                {
                    context.AddFailure(nameof(UpdateServiceCommandDto.Name), result.Error.Message);
                }
            });

        RuleFor(x => x.Price)
            .Custom((price, context) =>
            {
                var result = ServicePrice.Create(price);
                if (result.IsFailure)
                {
                    context.AddFailure(nameof(UpdateServiceCommandDto.Price), result.Error.Message);
                }
            });
    }
}