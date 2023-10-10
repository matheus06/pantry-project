using Microservice.PantryManager.Domain;
using Microservice.PantryManager.Domain.PantryAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Platform.Infra.Database.Abstractions;

namespace Microservice.PantryManager.Infra.Specifications;

public class PantryItemSpecification : Specification<PantryItem>
{
    public Guid PantryId { get; set; }
    public Guid ProductId { get; set; }


    public override IQueryable<PantryItem> AddPredicates(IQueryable<PantryItem> query)
    {
        if (Guid.Empty != (PantryId))
            query = query.Where(x => x.Id.Equals(PantryId));
        
        if (Guid.Empty != (ProductId))
            query = query.Where(x => x.ProductId.Equals(ProductId));

        return query;
    }


}