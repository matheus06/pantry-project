using Duende.IdentityServer.EntityFramework.DbContexts;
using Platform.Infra.Database;

namespace Microservice.IdentityServer.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddIoC(this IServiceCollection serviceCollection)
    {
        //DB init
        serviceCollection.AddTransient<DbInitializer>(x=> new DbInitializer(x.GetService<PersistedGrantDbContext>()) { IsToUseMigration = true });
        serviceCollection.AddTransient<DbInitializer>(x=> new DbInitializer(x.GetService<ConfigurationDbContext>()) { IsToUseMigration = true });

        
        return serviceCollection;
    }
}