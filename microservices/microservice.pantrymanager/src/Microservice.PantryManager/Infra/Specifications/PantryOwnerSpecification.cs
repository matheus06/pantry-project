using Microservice.PantryManager.Domain;
using Microservice.PantryManager.Domain.PantryAggregate.Entities;
using Platform.Infra.Database.Abstractions;

namespace Microservice.PantryManager.Infra.Specifications;

public class PantryOwnerSpecification : Specification<PantryOwner>
{ 
    public Guid PantryOwnerId  { get; set; }
    public string FirstNamePattern { get; set; }
    public string LastNamePattern { get; set; }
    public string EmailPattern { get; set; }

    public override IQueryable<PantryOwner> AddPredicates(IQueryable<PantryOwner> query)
    {
        if (Guid.Empty != (PantryOwnerId))
            query = query.Where(x => x.Id.Equals(PantryOwnerId));
        
        if (!string.IsNullOrWhiteSpace(FirstNamePattern))
            query = query.Where(x => x.FirstName.Contains(FirstNamePattern));
        
        if (!string.IsNullOrWhiteSpace(LastNamePattern))
            query = query.Where(x => x.LastName.Contains(LastNamePattern));
        
        if (!string.IsNullOrWhiteSpace(EmailPattern))
            query = query.Where(x => x.Email.Contains(EmailPattern));

        return query;
    }
}