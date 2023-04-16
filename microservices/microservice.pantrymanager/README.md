# Microservice Pantry


## Tech

* .Net 7
* Domain Layer with Exceptions and .NET Middleware as error Handler => `UseExcepitonHandler` with Minimal Api.
* App Layer using Custom exceptions and .NET Middleware as error Handler => `UseExcepitonHandler` with Minimal Api.
* Input validation using FluentValidation
* Dispatch before committing
* Persisting domain events along with domain objects. 
* Manual Mapping
* Dapr As EventBus and Dapr Apis to Handle Integration Events
* EF Core and code first.
* Health Check for Endpoint, Dapr and Database.
* Unit Tests
* Integration Tests
* Basic Metrics
* Serilog with ELK
* Authorization using Duende IdentityServer