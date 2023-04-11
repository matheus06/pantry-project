using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Platform.Infra.Database.Abstractions;
using Platform.Infra.Messaging;

namespace Platform.Infra.Database;

public class EntityFrameworkRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected DbContext DbContext { get; }
    private readonly IMediator _mediator;

    public EntityFrameworkRepository(DbContext dbContext, IMediator mediator)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mediator = mediator;
    }

    public Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (id == null)
            throw new ArgumentNullException(nameof(id));

        return DbContext.Set<TEntity>().FindAsync(new[] { id }, cancellationToken).AsTask();
    }

    public virtual Task<bool> ExistsAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (spec == null)
            throw new ArgumentNullException(nameof(spec));

        var query = DbContext.Set<TEntity>().AsQueryable();

        query = spec.AddPredicates(query);

        return query.AnyAsync(cancellationToken);
    }

    public virtual Task<TEntity> GetAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default(CancellationToken))
    {
        return BuildGetQuery(spec).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<TResult> GetAsync<TResult>(ISpecification<TEntity> spec, Expression<Func<TEntity, TResult>> projection,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        if (projection == null)
            throw new ArgumentNullException(nameof(projection));

        return BuildGetQuery(spec)
            .Select(projection)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<TEntity[]> ListAsync(ISpecification<TEntity> spec, int? skip = null, int? take = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        return BuildListQuery(spec, skip, take).ToArrayAsync(cancellationToken);
    }

    public Task<TResult[]> ListAsync<TResult>(ISpecification<TEntity> spec, Expression<Func<TEntity, TResult>> projection, int? skip = null, int? take = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        if (projection == null)
            throw new ArgumentNullException(nameof(projection));

        return BuildListQuery(spec, skip, take)
            .Select(projection)
            .ToArrayAsync(cancellationToken);
    }

    public Task<int> CountAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (spec == null)
            throw new ArgumentNullException(nameof(spec));

        var query = spec.AddPredicates(DbContext.Set<TEntity>().AsQueryable());

        return query.CountAsync(cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        DbContext.Set<TEntity>().Add(entity);
        
        await DispatchEvents();

        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        DbContext.Entry(entity).State = EntityState.Modified;
        
        await DispatchEvents();

        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        DbContext.Set<TEntity>().Remove(entity);
        
        await DispatchEvents();

        await DbContext.SaveChangesAsync(cancellationToken);
    }

    private IQueryable<TEntity> BuildGetQuery(ISpecification<TEntity> spec)
    {
        if (spec == null)
            throw new ArgumentNullException(nameof(spec));

        var query = DbContext.Set<TEntity>().AsQueryable();

        query = spec.AddPredicates(query);
        query = spec.AddSorting(query);
        query = spec.AddEagerFetching(query);

        return query;
    }

    private IQueryable<TEntity> BuildListQuery(ISpecification<TEntity> spec, int? skip, int? take)
    {
        if (spec == null)
            throw new ArgumentNullException(nameof(spec));

        if ((skip ?? 0) < 0)
            throw new ArgumentOutOfRangeException(nameof(skip));

        if (take.HasValue && take <= 0)
            throw new ArgumentOutOfRangeException(nameof(take));

        var query = BuildGetQuery(spec);

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        return query;
    }
    
    private async Task DispatchEvents()
    {
        // Dispatch Domain Events collection.
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB. This makes
        // a single transaction including side effects from the domain event
        // handlers that are using the same DbContext with Scope lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB. This makes
        // multiple transactions. You will need to handle eventual consistency and
        // compensatory actions in case of failures.
        await _mediator.DispatchDomainEventsAsync(DbContext);
    }
}