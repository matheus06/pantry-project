using MediatR;
using Microservice.ProductManager.Application.Behaviors;
using Microservice.ProductManager.Domain;
using Microservice.ProductManager.Infra.Database;
using Platform.Infra.Database;
using Platform.Infra.Database.Abstractions;
using Platform.Infra.Messaging;
using Platform.Infra.Messaging.Abstractions;

namespace Microservice.ProductManager.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddIoC(this IServiceCollection serviceCollection)
    {
        //DB init
        serviceCollection.AddTransient<DbInitializer>(x=> new DbInitializer(x.GetService<ProductContext>()));

        //Repositories
        serviceCollection.AddScoped<IRepository<Product>>(x =>
            new EntityFrameworkRepository<Product>(x.GetService<ProductContext>(), x.GetService<IMediator>()));
        
        //Add Bus
        serviceCollection.AddScoped<IEventBus, DaprEventBus>();

        serviceCollection.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        return serviceCollection;
    }
}