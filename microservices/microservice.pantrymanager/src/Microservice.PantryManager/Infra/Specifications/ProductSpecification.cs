using Microservice.PantryManager.Domain;
using Platform.Infra.Database.Abstractions;

namespace Microservice.PantryManager.Infra.Specifications;

public class ProductSpecification : Specification<Product>
{ 
    public string NamePattern { get; set; }
    
    public override IQueryable<Product> AddPredicates(IQueryable<Product> query)
    {
      
        if (!string.IsNullOrWhiteSpace(NamePattern))
            query = query.Where(x => x.Name.Contains(NamePattern));

        return query;
    }
}