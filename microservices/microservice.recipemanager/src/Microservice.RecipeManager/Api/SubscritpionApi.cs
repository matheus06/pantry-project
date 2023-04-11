using Dapr;
using Platform.Domain.Shared.IntegrationEvents;

namespace Microservice.RecipeManager.Api;

internal static class SubscriptionApi
{
    public static RouteGroupBuilder MapSubscription(this IEndpointRouteBuilder routes)
    {
        const string daprPubSubName = "pubsub";
        
        var group = routes.MapGroup("/subscription");
        group.WithTags("Subscription");
 
        // Dapr subscription in /dapr/subscribe sets up this route
        group.MapPost($"/{nameof(ProductCreatedIntegrationEvent)}", [Topic(daprPubSubName, nameof(ProductCreatedIntegrationEvent))] (ProductCreatedIntegrationEvent productCreatedIntegrationEvent) => {
            Console.WriteLine("ProductCreatedIntegrationEvent received => " +
                              $"EventId: {productCreatedIntegrationEvent.Id} " +
                              $"CreationDate: {productCreatedIntegrationEvent.CreationDate} " +
                              $"ProductId: {productCreatedIntegrationEvent.ProductId} " +
                              $"ProductName: {productCreatedIntegrationEvent.ProductName} " +
                              $"ProductDescription:  {productCreatedIntegrationEvent.ProductDescription}");
            return Results.Ok(productCreatedIntegrationEvent);
        }).ExcludeFromDescription();
        
        return group;
    }
}