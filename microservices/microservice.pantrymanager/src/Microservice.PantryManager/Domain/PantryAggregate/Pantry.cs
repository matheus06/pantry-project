using Microservice.PantryManager.Domain.PantryAggregate.Entities;
using Platform.Domain.Abstractions;

namespace Microservice.PantryManager.Domain.PantryAggregate;

public class Pantry : AggregateRoot<Guid>
{
    private readonly List<PantryItem> _pantryItems =  new();
    
    public string Name { get; private set; }
    public string Description { get; private set; }
    public  Guid PantryOwnerId { get; private set;  }
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }
    public IReadOnlyList<PantryItem> Items =>  _pantryItems.AsReadOnly();
    
    private Pantry() {}
    
    public Pantry(Guid pantryOwnerId, string pantryName, string pantryDescription):
        base(Guid.NewGuid())
    {
        if (Guid.Empty == pantryOwnerId)
            throw new ArgumentException(nameof(pantryOwnerId));
        
        PantryOwnerId = pantryOwnerId;
        Name = pantryName;
        Description = pantryDescription;
        CreatedDateTime = DateTime.UtcNow;
        UpdatedDateTime = DateTime.UtcNow;
    }
    
    public void AddItem(PantryItem pantryItem)
    {
        if (pantryItem == null)
            throw new ArgumentNullException(nameof(pantryItem));
        
        if(pantryItem.ItemQuantity.Amount <= 0)
            throw new InvalidOperationException("Can't Add Item with Quantity Less or Equal 0");
        
        var existingItem = Items.SingleOrDefault(item => item.ProductId == pantryItem.ProductId);
        if (existingItem != null)
        {
            existingItem.IncrementQuantity(pantryItem.ItemQuantity);
        }
        else
        {
            _pantryItems.Add(pantryItem);
        }
    }
    
    public void AddItems(IEnumerable<PantryItem> pantryItems)
    {
        if (pantryItems == null)
            throw new ArgumentNullException(nameof(pantryItems));
        
        _pantryItems.AddRange(pantryItems);
    }
    
    public void RemoveItem(Guid productId)
    {
        if (Guid.Empty == productId)
            throw new ArgumentNullException(nameof(productId));

        _pantryItems.RemoveAll(i => i.ProductId == productId);
    }

    public void Updated(string name, string description)
    {
        Name = name;
        Description = description;
        UpdatedDateTime = DateTime.UtcNow;
    }
}

