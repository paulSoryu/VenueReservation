
using FluentValidation;
using VenueReservation.Domain.Models.Services.ValueObjects;
using VenueReservation.Domain.Models.Venues.ValueObjects;

namespace VenueReservation.Application.Features.Venues.Commands.CreateVenue;

public class CreateVenueCommandValidator : AbstractValidator<CreateVenueCommand>
{
    public CreateVenueCommandValidator()
    {
        RuleFor(x => x.Name)
            .Custom((name, context) =>
            {
                var result = VenueName.Create(name);
                if (result.IsFailure)
                {
                    context.AddFailure(nameof(CreateVenueCommand.Name), result.Error.Message);
                }
            });

        RuleFor(x => x.Capacity)
            .Custom((capacity, context) =>
            {
                var result = Capacity.Create(capacity);
                if (result.IsFailure)
                {
                    context.AddFailure(nameof(CreateVenueCommand.Capacity), result.Error.Message);
                }
            });

        RuleFor(x => x.PricePerHour)
            .Custom((price, context) =>
            {
                var result = PricePerHour.Create(price);
                if (result.IsFailure)
                {
                    context.AddFailure(nameof(CreateVenueCommand.PricePerHour), result.Error.Message);
                }
            });

        RuleForEach(x => x.AvailableServices)
            .SetValidator(new CreateServiceCommandDtoValidator());
    }
}

public class CreateServiceCommandDtoValidator : AbstractValidator<CreateServiceCommandDto>
{
    public CreateServiceCommandDtoValidator()
    {
        RuleFor(x => x.Name)
            .Custom((name, context) =>
            {
                var result = ServiceName.Create(name);
                if (result.IsFailure)
                {
                    context.AddFailure(nameof(CreateServiceCommandDto.Name), result.Error.Message);
                }
            });

        RuleFor(x => x.Price)
            .Custom((price, context) =>
            {
                var result = ServicePrice.Create(price);
                if (result.IsFailure)
                {
                    context.AddFailure(nameof(CreateServiceCommandDto.Price), result.Error.Message);
                }
            });
    }
}