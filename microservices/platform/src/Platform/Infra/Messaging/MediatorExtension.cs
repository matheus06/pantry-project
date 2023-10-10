using MediatR;
using Microsoft.EntityFrameworkCore;
using Platform.Domain.Abstractions;

namespace Platform.Infra.Messaging;

public static class MediatorExtension
{
    public static async Task DispatchDomainEventsAsync<T>(this IMediator mediator, T ctx) where T : DbContext
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<IEntity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
}