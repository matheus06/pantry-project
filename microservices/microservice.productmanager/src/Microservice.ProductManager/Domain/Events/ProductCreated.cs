using Platform.Domain.Abstractions;

namespace Microservice.ProductManager.Domain.Events;

public class ProductCreated : DomainEvent
{ 
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    
    public ProductCreated(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}