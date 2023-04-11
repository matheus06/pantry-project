using Microservice.ProductManager.Domain.Events;
using Platform.Domain.Abstractions;

namespace Microservice.ProductManager.Domain;

public class Product : AggregateRoot<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    protected Product( ){ }

    public Product(string name, string description) 
        : base(Guid.NewGuid())
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(nameof(name));
        
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(nameof(description));
        
        Name = name;
        Description = description;
        
        AddDomainEvent(new ProductCreated(Id, Name, Description));
    }
}