﻿namespace Platform.Infra.Database.Abstractions;

public abstract class Specification<T> : ISpecification<T>
{
    public virtual IQueryable<T> AddEagerFetching(IQueryable<T> query)
    {
        return query ?? throw new ArgumentNullException(nameof(query));
    }

    public virtual IQueryable<T> AddPredicates(IQueryable<T> query)
    {
        return query ?? throw new ArgumentNullException(nameof(query));
    }

    public virtual IQueryable<T> AddSorting(IQueryable<T> query)
    {
        return query ?? throw new ArgumentNullException(nameof(query));
    }
}

