# Microservice Recipe


## Tech

* .Net 7
* Domain Layer using Exceptions, App Layer using Error Flow.
* Input validation using FluentValidation and generic ValidationBehavior for all Commands and Queries.
* Dispatch before committing
* Persisting domain events along with domain objects. 
* Manual Mapping
* Dapr As EventBus to publish the events
* EF Core and code first.
* Health Check for Endpoint, Dapr and Database.
* Unit Tests
* Integration Tests
* Basic Metrics
* Serilog with ELK
* Authorization using Duende IdentityServer