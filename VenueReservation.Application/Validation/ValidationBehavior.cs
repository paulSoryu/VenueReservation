
using FluentValidation;
using MediatR;
using System.Reflection;
using VenueReservation.Application.Validation.Errors;
using VenueReservation.Domain.Results;

namespace VenueReservation.Application.Validation;

// Restriction: works with any responses that inherit from your base Result class
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next();

        // 1. Launch FluentValidation for the incoming MediatR command
        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // 2. Collect all occurred errors
        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Any())
        {
            // 3. Group errors by fields: [FieldName -> Array of error messages]
            var errorsDictionary = failures
                .GroupBy(f => f.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(f => f.ErrorMessage).ToArray()
                );

            // 4. Create your custom ValidationError at the Application level
            var validationError = new ValidationError("Error in validating request data", errorsDictionary);

            // 5. Dynamically call the static Failure method of your Result / Result<T> class
            // This is a universal way to return a failed result for any TResponse type in MediatR
            var failureMethod = typeof(TResponse).GetMethod("Failure", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            if (failureMethod != null)
            {
                var failureResult = failureMethod.Invoke(null, [validationError]);
                return (TResponse)failureResult!;
            }

            throw new InvalidOperationException($"Type {typeof(TResponse).Name} must implement a static Failure method.");
        }

        return await next();
    }
}