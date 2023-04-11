using MediatR;
using Microservice.ProductManager.Application.Dto;
using Platform.ErrorHandling;

namespace Microservice.ProductManager.Api;

internal static class ProductApi
{
    public static RouteGroupBuilder MapProduct(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/product");
        group.WithTags("Product");

        group.MapPost("/", async Task<IResult> (ProductRequest productRequest, IMediator mediator) =>
        {
            var result = await mediator.Send(Mapper.MapToProductCommand(productRequest));
            return result.IsFailed ? ProblemHelper.Problem(result.Errors) : TypedResults.Created($"/product/{result.Value.Id}", result.Value);
        });
        
        return group;
    }


}