using Platform.Infra.Database;
using Microservice.Scheduler.Application.Services;
using Microservice.Scheduler.Infra.Database;
using Platform.Infra.Messaging;
using Platform.Infra.Messaging.Abstractions;

namespace Microservice.Scheduler.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddIoC(this IServiceCollection serviceCollection)
    {
        //DB init
        serviceCollection.AddTransient<DbInitializer>(x=> new DbInitializer(x.GetService<SchedulerContext>()));

        //Application Services
        serviceCollection.AddScoped<SchedulerService>();
        
        //Add Bus
        serviceCollection.AddScoped<IEventBus, DaprEventBus>();
        //serviceCollection.AddScoped<IJob, CallBackJob>();

        return serviceCollection;
    }
}