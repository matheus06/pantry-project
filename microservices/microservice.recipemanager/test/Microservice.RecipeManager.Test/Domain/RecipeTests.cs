using AutoFixture;
using Microservice.RecipeManager.Domain;
using Microservice.RecipeManager.Domain.RecipeAggregate;
using Microservice.RecipeManager.Domain.RecipeAggregate.Entities;
using Microservice.RecipeManager.Domain.RecipeAggregate.ValueObjects;

namespace Microservice.RecipeManager.Test.Domain;

public class RecipeTest
{
    private readonly IFixture _fixture;
    
    public RecipeTest()
    {
        _fixture = new Fixture();
    }
    
    [Fact]
    public void NewRecipe_WhenCreated_TypeIsEqualRecipe()
    {
       var recipe = new Recipe("RecipeName", "recipeDescription", "recipeInstructions");
        
       Assert.IsType<Recipe>(recipe);
    }
    
    [Fact]
    public void NewRecipe_WhenCreated_GuidIsNotEmpty()
    {
        var recipe = new Recipe("RecipeName", "recipeDescription", "recipeInstructions");
        
        Assert.NotEqual(recipe.Id,Guid.Empty);
    }
    
    [Fact]
    public void NewRecipe_WhenNameIsEmpty_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>  new Recipe(string.Empty, "recipeDescription", "recipeInstructions"));
    }
    
    [Fact]
    public void NewRecipe_WhenDescriptionIsEmpty_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>  new Recipe("RecipeName", string.Empty, "recipeInstructions"));
    }
    
    [Fact]
    public void NewRecipe_WhenInstructionsAreEmpty_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>  new Recipe("RecipeName", "recipeDescription", string.Empty));
    }
    
    [Fact]
    public void NewRecipe_WhenCreated_NameIsAssigned()
    {
        var recipe = new Recipe("RecipeName", "recipeDescription", "recipeInstructions");
        
        Assert.Equal("RecipeName", recipe.Name);
    }
    
    [Fact]
    public void NewRecipe_WhenCreated_DescriptionIsAssigned()
    {
        var recipe = new Recipe("RecipeName", "recipeDescription", "recipeInstructions");
        
        Assert.Equal("recipeDescription", recipe.Description);
    }
    
    [Fact]
    public void NewRecipe_WhenCreated_InstructionsAreAssigned()
    {
        var recipe = new Recipe("RecipeName", "recipeDescription", "recipeInstructions");
        
        Assert.Equal("recipeInstructions", recipe.Instructions);
    }
    
    [Fact]
    public void AddItem_WhenPantryItemIsAdded_ItemsCountHasTheSameQuantity()
    {
        var recipe = new Recipe("RecipeName", "recipeDescription", "recipeInstructions");
        var ingredient = _fixture.Create<Ingredient>();
        var quantity = _fixture.Create<Quantity>();
        
        recipe.AddIngredient(ingredient.Id, quantity);
        
        Assert.Equal(1,recipe.Ingredients.Count);
    }
    
    // [Fact]
    // public void AddItem_InputIsNull_ThrowsArgumentException()
    // {
    //     var pantry = new Pantry(Guid.NewGuid(), "PantryName", "pantryDescription");
    //     
    //     Assert.Throws<ArgumentNullException>(() => pantry.AddItem(null));
    // }
    //
    // [Fact]
    // public void AddItems_InputIsNull_ThrowsArgumentException()
    // {
    //     var pantry = new Pantry(Guid.NewGuid(), "PantryName", "pantryDescription");
    //     
    //     Assert.Throws<ArgumentNullException>(() => pantry.AddItems(null));
    // }
    //
    // [Fact]
    // public void RemoveItem_InputIsNull_ThrowsArgumentException()
    // {
    //     var pantry = new Pantry(Guid.NewGuid(), "PantryName", "pantryDescription");
    //     
    //     Assert.Throws<ArgumentNullException>(() => pantry.RemoveItem(null));
    // }
    //
    // [Fact]
    // public void RemoveItem_WhenPantryItemIsAdded_ThrowsArgumentException()
    // {
    //     var pantry = new Pantry(Guid.NewGuid(), "PantryName", "pantryDescription");
    //     var pantryItem = _fixture.Create<PantryItem>();
    //     
    //     pantry.AddItem(pantryItem);
    //     pantry.RemoveItem(pantryItem);
    //     
    //     Assert.Equal(0,pantry.Items.Count);
    // }
    //
    // [Fact]
    // public void AddItems_WhenPantryItemsAreAdded_ItemsCountHasTheSameQuantity()
    // {
    //     var pantry = new Pantry(Guid.NewGuid(), "PantryName", "pantryDescription");
    //     var pantryItems = _fixture.CreateMany<PantryItem>().ToList();
    //     
    //     pantry.AddItems(pantryItems);
    //     
    //     Assert.Equal(pantryItems.Count,pantry.Items.Count);
    // }
    //
    // [Fact]
    // public void AddItems_WhenOneItemIsRemoved_ItemsCountHasTheExpectedQuantity()
    // {
    //     var pantry = new Pantry(Guid.NewGuid(), "PantryName", "pantryDescription");
    //     var pantryItems = _fixture.CreateMany<PantryItem>().ToList();
    //     pantry.AddItems(pantryItems);
    //     
    //     pantry.RemoveItem(pantryItems.First());
    //     
    //     Assert.Equal(pantryItems.Count - 1,pantry.Items.Count);
    // }
    //
    // [Fact]
    // public void AddItems_WhenOneNonexistentItemIsRemoved_ItemsCountHasTheSameQuantity()
    // {
    //     var pantry = new Pantry(Guid.NewGuid(), "PantryName", "pantryDescription");
    //     var pantryItems = _fixture.CreateMany<PantryItem>().ToList();
    //     pantry.AddItems(pantryItems);
    //     
    //     pantry.RemoveItem(_fixture.Create<PantryItem>());
    //     
    //     Assert.Equal(pantryItems.Count,pantry.Items.Count);
    // }
}