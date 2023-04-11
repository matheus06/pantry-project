using Microservice.Scheduler.Application.Dto;
using Platform.Domain.Shared.IntegrationEvents;
using Platform.Infra.Messaging.Abstractions;

namespace Microservice.Scheduler.Application.Jobs;

public class BusCallBackJob
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<BusCallBackJob> _logger;

    public BusCallBackJob(IEventBus eventBus, ILogger<BusCallBackJob> logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }
    
    public void PublishMessage(BusCallBackJobRequest jobRequest)
    {
        _logger.LogInformation("Publishing RecurrentJobTriggeredIntegrationEvent Topic Name: {TopicName} and JobName:{Name}", jobRequest.TopicName, jobRequest.Name);
        _eventBus.Publish(new RecurrentJobIntegrationEvent(jobRequest.TopicName, jobRequest.Name, jobRequest.Payload));
        _logger.LogInformation("Published");
    }
}