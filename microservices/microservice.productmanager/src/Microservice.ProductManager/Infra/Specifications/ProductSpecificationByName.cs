using Microservice.ProductManager.Domain;
using Platform.Infra.Database.Abstractions;

namespace Microservice.ProductManager.Infra.Specifications;

public class ProductSpecificationByName : Specification<Product>
{ 
    public string NamePattern { get; set; }
    
    public override IQueryable<Product> AddPredicates(IQueryable<Product> query)
    {
      if (!string.IsNullOrWhiteSpace(NamePattern))
            query = query.Where(x => x.Name.Contains(NamePattern));
      
      return query;
    }
}