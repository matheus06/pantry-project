using Platform.Domain.Abstractions;

namespace Microservice.PantryManager.Domain;

public class Product : AggregateRoot<Guid>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    
    public DateTime CreatedDateTime { get; private set; }
    
    public DateTime UpdatedDateTime { get; private set; }
    
    public Product() { }
    
    public Product(Guid id, string name, string description):
        base(id)
    {
        if (Guid.Empty == id)
            throw new ArgumentException(nameof(id));
        
        if (string.IsNullOrEmpty(name) )
            throw new ArgumentException(nameof(name));
        
        if (string.IsNullOrEmpty(description) )
            throw new ArgumentException(nameof(description));
        
        Name = name;
        Description = description;
        CreatedDateTime = DateTime.UtcNow;
    }
}