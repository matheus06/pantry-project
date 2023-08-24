# Microservice Pantry

## Service Intro

## Tech Details

* .Net 7
* Domain Layer with `Exceptions` and `.NET Middleware as error Handler` => `UseExcepitonHandler` with Minimal Api.
* App Layer using `Custom exceptions` and .`NET Middleware as error Handler` => `UseExcepitonHandler` with Minimal Api.
* Input validation using `FluentValidation`.
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
