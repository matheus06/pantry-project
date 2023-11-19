using System.Reflection;
using FluentValidation;
using Microservice.ProductManager.Api;
using Microservice.ProductManager.Api.Validations;
using Microservice.ProductManager.Application.Queries;
using Microservice.ProductManager.Infra;
using Microservice.ProductManager.Infra.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Platform;
using Platform.Infra.Database;
using Platform.Infra.Messaging;
using Platform.Security;

var builder = WebApiApplicationBuilder.Build<Program>(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

//SQL Server
builder.Services.AddDbContext<ProductContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProductContext")));

//MediatR for Commands/Queries
builder.Services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

//Dapr for Bus
builder.Services.AddDaprClient();

//Add IoC
builder.Services.AddIoC();

//Health Checks
builder.Services.AddHealthChecks()
    // Add a health check for a SQL Server database
    .AddCheck(
        "database-product-check",
        new SqlConnectionHealthCheck(builder.Configuration.GetConnectionString("ProductContext")),
        HealthStatus.Unhealthy)
    .AddCheck<DaprHealthCheck>("dapr-check");

//GraphQL
builder.Services.AddGraphQLServer().AddQueryType<ProductGraphQuery>().AddProjections().AddFiltering().AddSorting();

//MediatR validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();

var app = builder.Build();

app.ConfigureBaseApplicationBuilders();
app.ConfigureBaseEndpointBuilders();

// Configure the Minimal APIs
app.MapProduct().RequireAuthorization(PolicyNames.ServiceScopes);

//Initialize DB
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
services.GetRequiredService<DbInitializer>().Run();

//Auths
app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

// Capture metrics about all received HTTP requests.
//app.UseHttpMetrics();
//app.MapMetrics();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }