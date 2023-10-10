using MediatR;
using Microservice.PantryManager.Application.Services;
using Microservice.PantryManager.Domain;
using Microservice.PantryManager.Domain.PantryAggregate;
using Microservice.PantryManager.Domain.PantryAggregate.Entities;
using Microservice.PantryManager.Infra.Database;
using Platform.Infra.Database;
using Platform.Infra.Database.Abstractions;
using Platform.Infra.Messaging;
using Platform.Infra.Messaging.Abstractions;

namespace Microservice.PantryManager.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddIoC(this IServiceCollection serviceCollection)
    {
        //DB init
        serviceCollection.AddTransient<DbInitializer>(x=> new DbInitializer(x.GetService<PantryContext>()));

        //Application Services
        serviceCollection.AddScoped<PantryService>();
        serviceCollection.AddScoped<PantryOwnerService>();
        serviceCollection.AddScoped<ProductService>();

        //Repositories
        serviceCollection.AddScoped<IRepository<Pantry>>(x =>
            new EntityFrameworkRepository<Pantry>(x.GetService<PantryContext>(), x.GetService<IMediator>()));
        serviceCollection.AddScoped<IRepository<PantryOwner>>(x =>
            new EntityFrameworkRepository<PantryOwner>(x.GetService<PantryContext>(), x.GetService<IMediator>()));
        serviceCollection.AddScoped<IRepository<PantryItem>>(x =>
            new EntityFrameworkRepository<PantryItem>(x.GetService<PantryContext>(), x.GetService<IMediator>()));
        serviceCollection.AddScoped<IRepository<Product>>(x =>
            new EntityFrameworkRepository<Product>(x.GetService<PantryContext>(), x.GetService<IMediator>()));
        
        //Add Bus
        serviceCollection.AddScoped<IEventBus, DaprEventBus>();

        return serviceCollection;
    }
}