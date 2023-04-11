using FluentResults;
using FluentValidation;
using MediatR;
using Platform.ErrorHandling.ApplicationErrors;

namespace Microservice.ProductManager.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest,TResponse> 
    where TRequest : IRequest<TResponse>
    where TResponse : IResultBase
{
    private readonly IValidator<TRequest> _validator;

    public ValidationBehavior(IValidator<TRequest> validator = null)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if(_validator is null)
            return await next();
        
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid)
            return await next();

        var errors = validationResult.Errors.ConvertAll(v =>new ValidationError(v.PropertyName, v.ErrorMessage));
        return (dynamic)Result.Fail(errors);
    }
}