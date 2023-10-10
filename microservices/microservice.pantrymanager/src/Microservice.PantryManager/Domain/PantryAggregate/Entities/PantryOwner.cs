using Microservice.PantryManager.Domain.PantryAggregate.ValueObjects;
using Platform.Domain.Abstractions;

namespace Microservice.PantryManager.Domain.PantryAggregate.Entities;

public class PantryOwner : Entity<Guid>
{ 
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }

    public PantryOwner() { }
    
    public PantryOwner(string firstname, string lastname, string email)
        : base(Guid.NewGuid())
    {
        if (string.IsNullOrEmpty(firstname))
            throw new ArgumentNullException(nameof(firstname));

        if (string.IsNullOrEmpty(lastname))
            throw new ArgumentNullException(nameof(lastname));

        if (string.IsNullOrEmpty(email))
            throw new ArgumentNullException(nameof(email));
        
        FirstName = firstname;
        LastName = lastname;
        Email = email;
    }
}