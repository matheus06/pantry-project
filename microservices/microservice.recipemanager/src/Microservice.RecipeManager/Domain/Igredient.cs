using Platform.Domain.Abstractions;

namespace Microservice.RecipeManager.Domain;

public class Ingredient : AggregateRoot<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    protected Ingredient( ){ }

    public Ingredient(string ingredientName, string ingredientDescription) 
        : base(Guid.NewGuid())
    {
        if (string.IsNullOrWhiteSpace(ingredientName))
            throw new ArgumentException(nameof(ingredientName));
        
        if (string.IsNullOrWhiteSpace(ingredientDescription))
            throw new ArgumentException(nameof(ingredientDescription));
        
        Name = ingredientName;
        Description = ingredientDescription;
    }
}