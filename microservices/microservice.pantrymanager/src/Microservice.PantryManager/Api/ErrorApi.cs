using Microsoft.AspNetCore.Diagnostics;
using Platform.Exceptions;

namespace Microservice.PantryManager.Api;

internal static class ErrorApi
{
    public static RouteGroupBuilder MapError(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/error");
        group.WithTags("Error");

        group.Map("/", (HttpContext context)=>
        {
            Exception exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

            var (statusCode, message) = exception switch
            {
                ICustomException customException => ((int)customException.StatusCode, customException.ErrorMessage),
                _ => (StatusCodes.Status500InternalServerError, exception?.Message),
            };
            
            return Results.Problem(title: message, statusCode: statusCode);
        });
        
        return group;
    }
}