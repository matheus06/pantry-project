using MediatR;
using Microservice.RecipeManager.Domain.RecipeAggregate;
using Microservice.RecipeManager.Infra.Database;
using Platform.Infra.Database;
using Platform.Infra.Database.Abstractions;

namespace Microservice.RecipeManager.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddIoC(this IServiceCollection serviceCollection)
    {
        //DB init
        serviceCollection.AddTransient<DbInitializer>(x=> new DbInitializer(x.GetService<RecipeContext>()));

        //Repositories
        serviceCollection.AddScoped<IRepository<Recipe>>(x =>
            new EntityFrameworkRepository<Recipe>(x.GetService<RecipeContext>(), x.GetService<IMediator>()));
        
        return serviceCollection;
    }
}