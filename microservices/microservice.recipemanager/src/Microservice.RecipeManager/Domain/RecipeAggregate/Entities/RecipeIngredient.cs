using Microservice.RecipeManager.Domain.RecipeAggregate.ValueObjects;
using Platform.Domain.Abstractions;

namespace Microservice.RecipeManager.Domain.RecipeAggregate.Entities;

public class RecipeIngredient : Entity<Guid>
{
    public Guid RecipeId { get; set; }
    public Guid  IngredientId { get; set; }
    public Quantity IngredientQuantity { get; set; }

    protected RecipeIngredient() {}
    public RecipeIngredient(Guid recipeId,  Guid ingredientId, Quantity quantity)
    {
        RecipeId = recipeId;
        IngredientId = ingredientId;
        IngredientQuantity = quantity;
    }
}