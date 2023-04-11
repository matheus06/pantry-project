# Pantry Project

Microservice.PantryManager
Microservice.ProductManager
Microservice.RecipeManager
Platform

Hexagonal (or onion) architecture.

## DDD

Presentation(Api)
Application
Domain
Infra

## DDD Aggregate

Aggregates are one of the more challenging aspects of tactical modeling. Developers often end up with large clusters of objects that do not give good performance and scalability.

The aggregate pattern discusses composition and alludes to information hiding. It also discusses consistency boundaries and transactions.

Rule: Model True Invariants In Consistency Boundaries
* An invariant is a business rule that must always be consistent
* When the transaction commits, everything inside one boundary must be consistent.
* Aggregates are chiefly about consistency boundaries and not driven by a desire to design object graphs.

Rule: Design Small Aggregates
* Many as possible?

Rule: Reference Other Aggregates By Identity
* Aggregates wiht ingerred object references are thus automatically smaller because references are never eagerly.
* Having am application service resolve depndencies frees the aggregate from relying on either a repository or a domain service.

Rule: User Eventual Consistency Outside the Boundary

Cost of memory vs lazy loading?
    * BOTE: Estimating Cost


### DDD Value Objects

There are two main characteristics for value objects:
* They have no identity.
* They are immutable

## DTO

### Application Service Layer or Presentation/Api Layer?

Current Approach:
DTO on Application Layer

- Mapping to DTO on API will expose your Domain to the API layer? Make sense?
    * Client facing models typically reside in the UI layer as ViewModels or ApiModels, or they may be called DTOs (Data Transfer Objects)
    * As a general best practice, your API shouldn’t expose any implementation details of your application.
    * Your API and View Models Should Not Reference Domain Models
    * By using separate API models, you can ensure that your API is as simple as possible, making your consumers’ lives easier.


## Validations

An invariant violation in the domain model is an exceptional situation and should be met with throwing an exception.
When the data enters the domain model boundary, it is assumed to be valid and any violation of this assumption means that you’ve introduced a bug. Such bugs are exceptional situations — the application should fail fast as soon as it encounters one.

(Note that there are scenarios related to race conditions where the domain model should also use a return value instead of throwing an exception) ??????

On the other hand, there’s nothing exceptional in external input being incorrect. You shouldn’t throw exceptions in such cases and instead should use a Result class.

To remove text messages from the domain layer, use error codes instead of text and convert those code to text in the presentation layer


Current Approach:
Domain Layer and AppLayer
Input Missing (Will use FluentValidation)

-Links:
https://enterprisecraftsmanship.com/posts/always-valid-domain-model/
https://enterprisecraftsmanship.com/posts/validation-and-ddd/
https://lostechies.com/jimmybogard/2009/02/15/validation-in-a-ddd-world/

## Events

### Domain Event

Current Approach:
`Dispatch before committing` to their respective handlers. `The handlers are on the Application Layer using MediatR INotificationHandler`.
It can be useful at times but falls short when the side effects of processing the domain events span across multiple transactions.
Consumer of the raised domain event should send notification emails. By sending notification emails before ensuring the database transaction is completed, you are opening the application for potential inconsistency issues. (Email sent but transaction failed)

Other approachs:
- Committing before dispatching
You can still run into inconsistencies.
If your email grid fails to accept the notification email, you will end up with a submitted order but with no confirmation email.

- Persisting domain events along with domain objects. 
What you need to do is add a table to the database and persist events into it like regular domain objects. Then, have a separate worker that picks those events one by one and processes them. 
In most cases that processing would involve pushing a message on a bus which then can fan it to the appropriate subscribers. But you can also come up with your own pub-sub mechanism.

-Links:
https://lostechies.com/jimmybogard/2014/05/13/a-better-domain-events-pattern/
https://enterprisecraftsmanship.com/posts/domain-events-simple-reliable-solution/
https://github.com/EduardoPires/EquinoxProject
https://github.com/NetDevPack/NetDevPack/tree/master/src/NetDevPack
https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-events-design-implementation

### Integration Event

It's important to mention that you might sometimes want to propagate events across multiple microservices. That propagation is an integration event, and it could be published through an event bus from any specific domain event handler.

Current Approach:
Dapr As EventBus and Dapr Apis to Handle Integration Events
CloudEvents 1.0 specification
`tye run` to lauch Dapr with all microservices

-Links:
https://betterprogramming.pub/domain-driven-design-domain-events-and-integration-events-in-net-5a2a58884aaa
https://github.com/dotnet-architecture/eShopOnDapr
https://github.com/dapr/quickstarts/tree/master/pub_sub/csharp
https://docs.dapr.io/developing-applications/sdks/dotnet/dotnet-development/dotnet-development-tye/


## Test

unit
integration

-Links:
https://github.com/DamianEdwards/MinimalApiPlayground


## Error Handling

Current Approach:
.NET Middleware => `UseExcepitonHandler` with Minimal Api

Other Approaches:
Filter
Custom Middleware
Change Flow Control

### Filter and Middleware

Middleware can be used for the entire request pipeline but Filters is only used within the Routing Middleware where we have an MVC pipeline so Middleware operates at the level of ASP.NET Core but Filters executes only when requests come to the MVC pipeline.

So if you don't require the context of MVC (let's say you're concerned about flow and execution, like responding to headers some pre-routing mechanism, etc.) then use middlewares.
But if you require the context of MVC and you want to operate against actions then use filters.


## Flow Control

Curent Approach:
Exceptions flow

Other approachs:
Oneof ? Good case Instead of throw excption?
FluentResult
ErrorOr


## Mapping

Current Approach:
- Manual Mapping

Other Aprroaches:
AutoMapper and other libraries
Should we use it?

## Database

https://jasontaylor.dev/ef-core-database-initialisation-strategies/
https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects
https://community.abp.io/posts/many-to-many-relationship-with-abp-and-ef-core-g7rm2qut

## CQRS


CQS: Method can either change state or return a value - not both
CQRS: Like CQS, but not as strict regarding the return value, the main emphasis is on having a clear boundary between Commands and Queries
Mediator Pattern: Promote loose coupling between objects by having them interact via a mediator rather than referencing each other
MediatR: An in-memory implementation of the Mediator pattern, where MediatR requests & MediatR handlers are wired up during the DI setup
Splitting Logic By Feature: Having each use case in a separate file

Grow Horizontaly by Business Cases

Command
    BusinessX
        .....
    BusinessY
        .....
Query
    Business

## Tye

dotnet tool install -g Microsoft.Tye --version "0.11.0-alpha.22111.1"

tye run .\Microservice.PantryManager.csproj
tye run .\Microservice.PantryManager.csproj
or
Create yaml with services
tye run

## Health Checks

https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-7.0
https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/monitor-app-health
https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/issues/712
https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/blob/master/README.md
https://github.com/dotnet/tye/blob/main/docs/reference/service_discovery.md

###########################################################################################################################################################################################################################################################################################################

Todo

InputValidation - flunetvalidation
second approach ErrorOr in different MS.....

Prometheus
Identity Server (iodc)
Logg
GraphQL
Sagas


## Tracing - Latter

In order to make a system observable, it must be instrumented. That is, the code must emit traces, metrics, and logs.
The instrumented data must then be sent to an Observability back-end.
OpenTelemetry is not an observability back-end like Jaeger or Prometheus. Instead, it supports exporting data to a variety of open source and commercial back-ends.

logs, metrics and spans

latter


docker run -p 9090:9090 prom/prometheus
docker run -d --name jaeger  -e COLLECTOR_ZIPKIN_HOST_PORT=:9411  -e COLLECTOR_OTLP_ENABLED=true -p 6831:6831/udp  -p 6832:6832/udp  -p 5778:5778  -p 16686:16686 -p 4317:4317  -p 4318:4318  -p 14250:14250  -p 14268:14268  -p 14269:14269  -p 9411:9411  jaegertracing/all-in-one:1.43


### Event Sourcing