using Microservice.Scheduler.Application;
using Microservice.Scheduler.Application.Jobs;
using Microservice.Scheduler.Application.Services;
using Platform.Infra.Messaging;
using Platform.Infra.Messaging.Abstractions;

namespace Microservice.Scheduler.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddIoC(this IServiceCollection serviceCollection)
    {
        //Application Services
        serviceCollection.AddScoped<SchedulerService>();
        
        //Add Bus
        serviceCollection.AddScoped<IEventBus, DaprEventBus>();
        //serviceCollection.AddScoped<IJob, CallBackJob>();

        return serviceCollection;
    }
}