namespace Platform.Infra.Messaging.Abstractions;

public interface IEventBus
{
    Task Publish(IntegrationEvent @event);
}