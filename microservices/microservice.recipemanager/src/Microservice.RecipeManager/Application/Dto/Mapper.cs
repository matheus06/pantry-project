using MediatR;
using Microservice.RecipeManager.Application.Queries;
using Microservice.RecipeManager.Domain;
using Microservice.RecipeManager.Domain.RecipeAggregate;

namespace Microservice.RecipeManager.Application.Dto;

public static class Mapper
{
    public static RecipeResponse MapToRecipeResponse(Recipe recipe)
    {
        return new RecipeResponse
        {
            Id = recipe.Id,
            Description = recipe.Description, 
            Instructions = recipe.Instructions,
            CreatedDateTime  = recipe.CreatedDateTime,
            UpdatedDateTime  = recipe.UpdatedDateTime
        };
    }

    public static GetRecipeQuery MapToRecipeQuery(Guid recipeId)
    {
        return new GetRecipeQuery(recipeId);
    }
}