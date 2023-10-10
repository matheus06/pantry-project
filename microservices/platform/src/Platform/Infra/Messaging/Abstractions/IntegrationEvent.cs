namespace Platform.Infra.Messaging.Abstractions;

public record IntegrationEvent
{
    public Guid Id { get; } = Guid.NewGuid();

    public DateTime CreationDate { get; } = DateTime.UtcNow;
    
    public string TopicName { get; protected set; }
}