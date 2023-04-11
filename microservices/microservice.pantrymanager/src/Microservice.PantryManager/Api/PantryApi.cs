using FluentValidation;
using Microservice.PantryManager.Application.Dto;
using Microservice.PantryManager.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.PantryManager.Api;

internal static class PantryApi
{
    public static RouteGroupBuilder MapPantry(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/pantry");
        group.WithTags("Pantry");

        group.MapGet("/{pantryId:guid}", async Task<IResult>(Guid pantryId,
            [FromServices] PantryService pantryService, [FromServices] IValidator<Guid> validator) =>
        {
            var validation = await validator.ValidateAsync(pantryId);
            if (!validation.IsValid)
                return TypedResults.ValidationProblem(validation.ToDictionary());
            
            var pantry = await pantryService.GetPantry(pantryId);
            return TypedResults.Ok(pantry);
        });
        
        group.MapPost("/", async Task<IResult>(PantryCreateRequest pantryCreateRequest,
                [FromServices] PantryService pantryService, [FromServices] IValidator<PantryCreateRequest> validator) =>
        {
            var validation = await validator.ValidateAsync(pantryCreateRequest);
            if (!validation.IsValid)
                return TypedResults.ValidationProblem(validation.ToDictionary());

            var createdPantry = await pantryService.CreatePantry(pantryCreateRequest);
            return TypedResults.Created($"/pantry/{createdPantry.Id}", createdPantry);
        });
        
        group.MapPut("/", async Task<IResult>(PantryUpdateRequest pantryUpdateRequest,
            [FromServices] PantryService pantryService, [FromServices] IValidator<PantryUpdateRequest> validator) =>
        {
            var validation = await validator.ValidateAsync(pantryUpdateRequest);
            if (!validation.IsValid)
                return TypedResults.ValidationProblem(validation.ToDictionary());

            var updatedPantry = await pantryService.UpdatePantry(pantryUpdateRequest);
            return TypedResults.Ok(updatedPantry);
        });
        
        group.MapPost("/{pantryId:guid}/item", async Task<IResult>(Guid pantryId, AddPantryItemRequest addPantryItemRequest,
            [FromServices] PantryService pantryService, [FromServices] IValidator<AddPantryItemRequest> validator,  [FromServices] IValidator<Guid> guidValidator) =>
        {
            var pantryIdValidation = await guidValidator.ValidateAsync(pantryId);
            if (!pantryIdValidation.IsValid)
                return TypedResults.ValidationProblem(pantryIdValidation.ToDictionary());
            
            var validation = await validator.ValidateAsync(addPantryItemRequest);
            if (!validation.IsValid)
                return TypedResults.ValidationProblem(validation.ToDictionary());

            var createdPantry = await pantryService.AddPantryItem(pantryId, addPantryItemRequest);
            return TypedResults.Ok(createdPantry);
        });
        
        group.MapDelete("/{pantryId:guid}/item/{productId:guid}", async Task<IResult>(Guid pantryId, Guid productId,
            [FromServices] PantryService pantryService, [FromServices] IValidator<Guid> validator) =>
        {
            var pantryIdValidation = await validator.ValidateAsync(pantryId);
            if (!pantryIdValidation.IsValid)
                return TypedResults.ValidationProblem(pantryIdValidation.ToDictionary());
            
            var productIdValidation = await validator.ValidateAsync(productId);
            if (!productIdValidation.IsValid)
                return TypedResults.ValidationProblem(productIdValidation.ToDictionary());
            
            var deletePantryItem = await pantryService.DeletePantryItem(pantryId, productId);
            return TypedResults.Ok(deletePantryItem);
        });
        
        return group;
    }
}