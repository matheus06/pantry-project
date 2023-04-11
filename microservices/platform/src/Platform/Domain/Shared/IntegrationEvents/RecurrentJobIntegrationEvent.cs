using Platform.Infra.Messaging.Abstractions;

namespace Platform.Domain.Shared.IntegrationEvents;

public record RecurrentJobIntegrationEvent : IntegrationEvent
{
    public string JobName { get; private set; }
    public string Payload { get; private set; }
    public RecurrentJobIntegrationEvent(string topicName, string jobName, string payload)
    {
        TopicName = topicName;
        JobName = jobName;
        Payload = payload;
    }
}