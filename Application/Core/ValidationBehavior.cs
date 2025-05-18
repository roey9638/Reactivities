using System;
using FluentValidation;
using MediatR;

namespace Application.Core;

// The [IValidator<TRequest>? validator] will get either the [Command] or [Query].
// The [ValidationBehavior] will be [execute] in the [Program] [Class] VVV
// And what's will be [PASS] into it will also come from [Program] [Class].
public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Here I'm checking if [validator == null]. If it is [null] it means that we are [NOT] [validating] anything.
        if (validator == null)
        {
            // The [next()] is to [call] the next [middleware] in the pipeline.
            return await next();
        }
        // In Here [await validator.ValidateAsync(request, cancellationToken)]
        // I'm [validating] that [everything] is how it should be.
        // The [request] is either will be the [Command] or the [Query] that we [validating].
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        // Here if we [valide] [something] and it's ["wrong"]. will just [throw] [ValidationException]
        if (!validationResult.IsValid)
        {
            // I'm [Handling] this [Exception] in the [ExceptionMiddleware] [Class].
            throw new ValidationException(validationResult.Errors);
        }

        return await next();
    }
}
