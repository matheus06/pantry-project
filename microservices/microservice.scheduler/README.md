# Microservice Scheduler

## Tech

* `.Net 7`
* Domain Layer using `Exceptions`
* App Layer using `FluentResult` and `Error Flow Control`.
* Input validation using `FluentValidation`.
* `Mediatr` to dispatch `Commands` and `Queries` and generic `ValidationBehavior` for them.
* `Mediatr` to dispatch `Domain Events` before committing.
* Persisting `Domain Events` along with `Domain Objects`.
* Manual Mapping between `DTO` and `Domain Objects`.
* `Dapr` As `EventBus` to publish\subscribe `Integration Events`.
* `EF Core` and code first.
* `Health Check` for Endpoint, Dapr and Database.
* Unit Tests
* Integration Tests
* Basic Metrics
* `Serilog` with `ELK`
* Authorization using `Duende IdentityServer`.