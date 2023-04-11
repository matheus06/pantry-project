using FluentValidation;
using Microservice.PantryManager.Application.Dto;
using Microservice.PantryManager.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.PantryManager.Api;

internal static class PantryOwnerApi
{
    public static RouteGroupBuilder MapPantryOwner(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/pantryOwner");
        group.WithTags("PantryOwner");

        group.MapPost("/", async Task<IResult> (PantryOwnerCreateRequest pantryOwnerCreateRequest, 
            [FromServices] PantryOwnerService pantryOwnerService, [FromServices] IValidator<PantryOwnerCreateRequest> validator) =>
        {
            var validation = await validator.ValidateAsync(pantryOwnerCreateRequest);
            if (!validation.IsValid)
                return TypedResults.ValidationProblem(validation.ToDictionary());
            
            var createdPantryOwner = await pantryOwnerService.CreatePantryOwner(pantryOwnerCreateRequest);
            return TypedResults.Created($"/pantry/{createdPantryOwner.Id}", createdPantryOwner);
        });
        
        return group;
    }
}