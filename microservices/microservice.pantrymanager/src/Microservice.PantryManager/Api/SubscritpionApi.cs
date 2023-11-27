using Microservice.PantryManager.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Platform.Domain.Shared.IntegrationEvents;

namespace Microservice.PantryManager.Api;

internal static class SubscriptionApi
{
    public static RouteGroupBuilder MapDaprSubscription(this IEndpointRouteBuilder routes, string pubSubName)
    {

        string daprPubSubName = Environment.GetEnvironmentVariable("PUBSUB_NAME") ?? pubSubName ?? "pubsub";

        var group = routes.MapGroup("/subscription");
        group.WithTags("Subscription");

        // Dapr subscription in /dapr/subscribe sets up this route
        group.MapPost($"/{nameof(ProductCreatedIntegrationEvent)}",
            async Task<IResult> (ProductCreatedIntegrationEvent @event, [FromServices] ProductService productService) => {
            Console.WriteLine("ProductCreatedIntegrationEvent received => " +
                              $"EventId: {@event.Id} " +
                              $"CreationDate: {@event.CreationDate} " +
                              $"ProductId: {@event.ProductId} " +
                              $"ProductName: {@event.ProductName} " +
                              $"ProductDescription:  {@event.ProductDescription}");

            await productService.CreateNewProduct(@event.ProductId, @event.ProductName, @event.ProductDescription);
            return Results.Ok(@event);
        }).ExcludeFromDescription().WithTopic(daprPubSubName, nameof(ProductCreatedIntegrationEvent));
        
        // Dapr subscription in /dapr/subscribe sets up this route
        group.MapPost("/RecurrentTopicTest",
            IResult (RecurrentJobIntegrationEvent @event, [FromServices] ProductService productService) =>
            {
                Console.WriteLine("RecurrentJobTriggeredIntegrationEvent received => " +
                                  $"EventId: {@event.Id} " +
                                  $"EventId: {@event.JobName} " +
                                  $"CreationDate: {@event.CreationDate}");
                return Results.Ok(@event);
            }).ExcludeFromDescription().WithTopic(daprPubSubName, "RecurrentTopicTest"); ;
        
        return group;
    }
}