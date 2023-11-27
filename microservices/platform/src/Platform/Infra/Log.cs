using Platform.Infra.Messaging.Abstractions;

namespace Platform.Infra;

static partial class Log
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Publishing event {@Event} to {PubSubName}.{TopicName}")]
    public static partial void EventPublished(this ILogger logger, IntegrationEvent @event, string pubSubName, string topicName);
}
