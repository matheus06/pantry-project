using Microservice.PantryManager.Domain.PantryAggregate;
using Platform.Infra.Database.Abstractions;

namespace Microservice.PantryManager.Infra.Specifications;

public class PantrySpecification : Specification<Pantry>
{ 
    public Guid PantryOwnerId  { get; set; }
    public string NamePattern { get; set; }
    
    public override IQueryable<Pantry> AddPredicates(IQueryable<Pantry> query)
    {
        if (Guid.Empty != PantryOwnerId)
            query = query.Where(x => x.PantryOwnerId.Equals(PantryOwnerId));
        
        if (!string.IsNullOrWhiteSpace(NamePattern))
            query = query.Where(x => x.Name.Contains(NamePattern));

        return query;
    }
}