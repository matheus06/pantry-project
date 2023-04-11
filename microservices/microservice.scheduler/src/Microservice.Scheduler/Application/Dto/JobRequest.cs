namespace Microservice.Scheduler.Application.Dto;

public abstract class  JobRequest
{
    public string Name { get; set; }
    public string CronExpression { get; set; }
    public string Payload { get; set; }
}
