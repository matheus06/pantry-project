using Dapr.Client;
using Platform.Infra.Messaging.Abstractions;

namespace Platform.Infra.Messaging;

public class DaprEventBus: IEventBus
{
    private readonly DaprClient _daprClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DaprEventBus> _logger;
    
    public DaprEventBus(DaprClient daprClient, IConfiguration configuration, ILogger<DaprEventBus> logger) 
    {
        _daprClient = daprClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task Publish(IntegrationEvent @event)
    {
        var pubSubName = Environment.GetEnvironmentVariable("PUBSUB_NAME") ?? _configuration["PubSubName"] ?? "pubsub";
        if (string.IsNullOrEmpty(pubSubName))
            throw new ArgumentNullException(nameof(pubSubName));
        
        var topicName = @event.TopicName ??  @event.GetType().Name;

        _logger.EventPublished(@event, pubSubName, topicName);
        
        // We need to make sure that we pass the concrete type to PublishEventAsync,
        // which can be accomplished by casting the event to dynamic. This ensures
        // that all event fields are properly serialized.
        await _daprClient.PublishEventAsync(pubSubName, topicName, (object)@event);
    }    
}
