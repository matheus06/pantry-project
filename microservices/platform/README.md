# Platfrom

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

## PATTERN: PLATFORM TEAM

Create a team to be in charge of architecting, building, and running a single, consistent,and stable cloud native platform for use by the entire organization so that developers canfocus on building applications instead of configuring infrastructure

https://www.cnpatterns.org/organization-culture/platform-team