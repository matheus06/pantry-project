using MediatR;
using Microservice.RecipeManager.Application.Commands.Recipe;
using Microservice.RecipeManager.Application.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Platform.ErrorHandling;

namespace Microservice.RecipeManager.Api;

internal static class RecipeApi
{
    public static RouteGroupBuilder MapRecipe(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/recipe");
        group.WithTags("Recipe");

        group.MapPost("/", async Task<Created<RecipeResponse>> (RecipeRequest recipeRequest, IMediator mediator) =>
        {
            var createdRecipe = await mediator.Send(new CreateRecipeCommand());
            return TypedResults.Created($"/recipe/{createdRecipe.Id}", createdRecipe);
        });
        
        group.MapGet("/{recipeId}", async Task<IResult> (Guid recipeId, IMediator mediator) =>
        {
            var result = await mediator.Send(Mapper.MapToRecipeQuery(recipeId));
            return result.IsFailed ? ProblemHelper.Problem(result.Errors) : TypedResults.Ok(result.Value);
        });
        
        return group;
    }
}