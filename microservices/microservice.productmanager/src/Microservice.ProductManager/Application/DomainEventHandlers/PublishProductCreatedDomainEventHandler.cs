using MediatR;
using Microservice.ProductManager.Domain.Events;
using Platform.Domain.Shared.IntegrationEvents;
using Platform.Infra.Messaging.Abstractions;

namespace Microservice.ProductManager.Application.DomainEventHandlers;

public class PublishProductCreatedDomainEventHandler : INotificationHandler<ProductCreated>
{
    private ILogger<PublishProductCreatedDomainEventHandler> _logger;
    private IEventBus _eventBus;
    
    public PublishProductCreatedDomainEventHandler(ILogger<PublishProductCreatedDomainEventHandler> logger, IEventBus eventBus)
    {
        _logger = logger;
        _eventBus = eventBus;
    }
    
    public async Task Handle(ProductCreated productCreated, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: ProductCreated Handled!");
        await _eventBus.Publish(new ProductCreatedIntegrationEvent(productCreated.Id, productCreated.Name, productCreated.Description));
        _logger.LogInformation("Integration Event Sent: ProductCreatedIntegrationEvent ");
    }
}