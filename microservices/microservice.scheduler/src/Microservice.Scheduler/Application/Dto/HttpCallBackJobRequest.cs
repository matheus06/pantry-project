namespace Microservice.Scheduler.Application.Dto;

public class HttpCallBackJobRequest : JobRequest
{
    public string Uri { get; set; }
}