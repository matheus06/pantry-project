using AutoFixture;
using Microservice.PantryManager.Domain.PantryAggregate;
using Microservice.PantryManager.Domain.PantryAggregate.Entities;

namespace Microservice.PantryManager.Test.Domain;

public class PantryTest
{
    private readonly IFixture _fixture;
    
    public PantryTest()
    {
        _fixture = new Fixture();
    }
    
    [Fact]
    public void NewPantry_WhenCreated_TypeIsEqualPantry()
    {
       var pantry = new Pantry(Guid.NewGuid(), "PantryName", "pantryDescription");
        
       Assert.IsType<Pantry>(pantry);
    }
    
    [Fact]
    public void NewPantry_WhenCreated_GuidIsNotEmpty()
    {
        var pantry = new Pantry(Guid.NewGuid(), "PantryName", "pantryDescription");
        
        Assert.NotEqual(pantry.Id,Guid.Empty);
    }
    
    [Fact]
    public void NewPantry_WhenPantryOwnerIdIsEmpty_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>  new Pantry(Guid.Empty, "PantryName", "pantryDescription"));
    }
    
    [Fact]
    public void NewPantry_WhenCreated_NameIsAssigned()
    {
        var pantry = new Pantry(Guid.NewGuid(), "PantryName", "pantryDescription");
        
        Assert.Equal("PantryName", pantry.Name);
    }
    
    [Fact]
    public void NewPantry_WhenCreated_DescriptionIsAssigned()
    {
        var pantry = new Pantry(Guid.NewGuid(), "PantryName", "pantryDescription");
        
        Assert.Equal("pantryDescription", pantry.Description);
    }
    
    [Fact]
    public void AddItem_WhenPantryItemIsAdded_ItemsCountHasTheSameQuantity()
    {
        var pantry = new Pantry(Guid.NewGuid(), "PantryName", "pantryDescription");
        var pantryItem = _fixture.Create<PantryItem>();
        
        pantry.AddItem(pantryItem);
        
        Assert.Equal(1,pantry.Items.Count);
    }
    
    [Fact]
    public void AddItem_InputIsNull_ThrowsArgumentException()
    {
        var pantry = new Pantry(Guid.NewGuid(), "PantryName", "pantryDescription");
        
        Assert.Throws<ArgumentNullException>(() => pantry.AddItem(null));
    }
    
    [Fact]
    public void AddItems_InputIsNull_ThrowsArgumentException()
    {
        var pantry = new Pantry(Guid.NewGuid(), "PantryName", "pantryDescription");
        
        Assert.Throws<ArgumentNullException>(() => pantry.AddItems(null));
    }
    
    [Fact]
    public void RemoveItem_InputIsNull_ThrowsArgumentException()
    {
        var pantry = new Pantry(Guid.NewGuid(), "PantryName", "pantryDescription");
        
        Assert.Throws<ArgumentNullException>(() => pantry.RemoveItem(Guid.Empty));
    }
    
    [Fact]
    public void AddItems_WhenPantryItemsAreAdded_ItemsCountHasTheSameQuantity()
    {
        var pantry = new Pantry(Guid.NewGuid(), "PantryName", "pantryDescription");
        var pantryItems = _fixture.CreateMany<PantryItem>().ToList();
        
        pantry.AddItems(pantryItems);
        
        Assert.Equal(pantryItems.Count,pantry.Items.Count);
    }
    
    [Fact]
    public void AddItems_WhenOneItemIsRemoved_ItemsCountHasTheExpectedQuantity()
    {
        var pantry = new Pantry(Guid.NewGuid(), "PantryName", "pantryDescription");
        var pantryItems = _fixture.CreateMany<PantryItem>().ToList();
        pantry.AddItems(pantryItems);
        
        pantry.RemoveItem(pantryItems.First().ProductId);
        
        Assert.Equal(pantryItems.Count - 1,pantry.Items.Count);
    }
    
    [Fact]
    public void AddItems_WhenOneNonexistentItemIsRemoved_ItemsCountHasTheSameQuantity()
    {
        var pantry = new Pantry(Guid.NewGuid(), "PantryName", "pantryDescription");
        var pantryItems = _fixture.CreateMany<PantryItem>().ToList();
        pantry.AddItems(pantryItems);
        
        pantry.RemoveItem(_fixture.Create<PantryItem>().ProductId);
        
        Assert.Equal(pantryItems.Count,pantry.Items.Count);
    }
}