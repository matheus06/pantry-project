using Microservice.PantryManager.Domain;
using Microservice.PantryManager.Domain.PantryAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Platform.Infra.Database.Abstractions;

namespace Microservice.PantryManager.Infra.Specifications;

public class ItemSpecification : Specification<PantryItem>
{
    public string NamePattern { get; set; }
    public bool SortByNameAsc { get; set; }

    public override IQueryable<PantryItem> AddEagerFetching(IQueryable<PantryItem> query)
    {
        return query;
    }

    public override IQueryable<PantryItem> AddPredicates(IQueryable<PantryItem> query)
    {
        // if (!string.IsNullOrWhiteSpace(NamePattern))
        //     query = query.Where(x => x.Name.Contains(NamePattern));

        return query;
    }

    public override IQueryable<PantryItem> AddSorting(IQueryable<PantryItem> query)
    {
        // return SortByNameAsc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
        return query;
    }
}