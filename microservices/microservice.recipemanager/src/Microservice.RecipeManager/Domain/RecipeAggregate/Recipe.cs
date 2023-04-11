using Microservice.RecipeManager.Domain.RecipeAggregate.Entities;
using Microservice.RecipeManager.Domain.RecipeAggregate.ValueObjects;
using Platform.Domain.Abstractions;

namespace Microservice.RecipeManager.Domain.RecipeAggregate;

public class Recipe : AggregateRoot<Guid>
{
    private readonly List<RecipeIngredient> _ingredients =  new();
    
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Instructions { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }
    public IReadOnlyList<RecipeIngredient> Ingredients =>  _ingredients.AsReadOnly();

    private Recipe() {}
    
    public Recipe(string recipeName, string recipeDescription, string recipeInstructions):
        base(Guid.NewGuid())
    {
        if (string.IsNullOrWhiteSpace(recipeName))
            throw new ArgumentException(nameof(recipeName));
        
        if (string.IsNullOrWhiteSpace(recipeDescription))
            throw new ArgumentException(nameof(recipeDescription));
        
        if (string.IsNullOrWhiteSpace(recipeInstructions))
            throw new ArgumentException(nameof(recipeInstructions));
        
        Name = recipeName;
        Description = recipeDescription;
        Instructions = recipeInstructions;
        CreatedDateTime = DateTime.Now;
        UpdatedDateTime = DateTime.Now;
    }
    
    public void AddIngredient(Guid ingredientId, Quantity quantity)
    {
        if (Guid.Empty == ingredientId)
            throw new ArgumentNullException(nameof(ingredientId));
        
        if (quantity == null)
            throw new ArgumentNullException(nameof(quantity));
        
        _ingredients.Add(new RecipeIngredient(Id, ingredientId, quantity));
    }
    
    public void RemoveIngredient(Guid ingredientId)
    {
        _ingredients.RemoveAll(ri=> ri.Id == ingredientId);
    }
    
    public void RemoveAllIngredients()
    {
        _ingredients.RemoveAll(ri=> ri.RecipeId == Id);
    }
}