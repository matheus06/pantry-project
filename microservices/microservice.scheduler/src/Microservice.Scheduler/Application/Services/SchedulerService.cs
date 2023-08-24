using Hangfire;
using Microservice.Scheduler.Application.Dto;
using Microservice.Scheduler.Application.Jobs;

namespace Microservice.Scheduler.Application.Services;

public class SchedulerService
{
    public void ScheduleJob(JobRequest jobRequest)
    {
        //Bus
        if (jobRequest is BusCallBackJobRequest busJob)
        {
            if (!string.IsNullOrEmpty(busJob.CronExpression))
            {
                RecurringJob.AddOrUpdate<BusCallBackJob>(busJob.Name, x => x.PublishMessage(busJob), busJob.CronExpression);
            }
            else
            {
                BackgroundJob.Enqueue<BusCallBackJob>(x => x.PublishMessage(busJob));
            }
        }
        
        //Http
        if (jobRequest is HttpCallBackJobRequest httpJob)
        {
            if (!string.IsNullOrEmpty(httpJob.CronExpression))
            {
                RecurringJob.AddOrUpdate<HttpCallBackJob>(httpJob.Name, x => x.PublishMessage(httpJob), httpJob.CronExpression);
            }
            else
            {
                BackgroundJob.Enqueue<HttpCallBackJob>(x => x.PublishMessage(httpJob));
            }
        }
    }
}


