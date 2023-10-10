namespace Microservice.Scheduler.Application.Dto;

public class BusCallBackJobRequest : JobRequest
{
    public string TopicName { get; set; }
}