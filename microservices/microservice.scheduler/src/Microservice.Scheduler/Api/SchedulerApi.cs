using Microservice.Scheduler.Application.Dto;
using Microservice.Scheduler.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Scheduler.Api;

internal static class SchedulerApi
{
    public static RouteGroupBuilder MapScheduler(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/scheduler");
        group.WithTags("Scheduler");
        
        group.MapPost("/busJob",  IResult (BusCallBackJobRequest busCallBackJobRequest,  [FromServices] SchedulerService schedulerService) =>
        {
            schedulerService.ScheduleJob(busCallBackJobRequest);  
            return TypedResults.NoContent();
        });
        
        group.MapPost("/httpJob",  IResult (HttpCallBackJobRequest httpCallBackJobRequest,  [FromServices] SchedulerService schedulerService) =>
        {
            schedulerService.ScheduleJob(httpCallBackJobRequest);  
            return TypedResults.NoContent();
        });
        
        return group;
    }
}