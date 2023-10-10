namespace Platform.Infra.Database.Abstractions;

public interface ISpecification<T>
{
    IQueryable<T> AddEagerFetching(IQueryable<T> query);
    IQueryable<T> AddPredicates(IQueryable<T> query);
    IQueryable<T> AddSorting(IQueryable<T> query);
}

