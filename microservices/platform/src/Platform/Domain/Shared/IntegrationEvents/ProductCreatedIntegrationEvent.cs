using Platform.Infra.Messaging.Abstractions;

namespace Platform.Domain.Shared.IntegrationEvents;

public record ProductCreatedIntegrationEvent(Guid ProductId, string ProductName, string ProductDescription) : IntegrationEvent;