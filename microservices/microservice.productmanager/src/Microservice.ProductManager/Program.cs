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

var builder = WebApiApplicationBuilder.Build<Program>(args);

//SQL Server
builder.Services.AddDbContext<ProductContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProductContext")));

//MediatR for Commands/Queries
builder.Services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

//Dapr for Bus
builder.Services.AddDaprClient();

//Add IoC
builder.Services.AddIoC();

builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();

//Health Checks
builder.Services.AddHealthChecks()
    // Add a health check for a SQL Server database
    .AddCheck(
        "database-product-check",
        new SqlConnectionHealthCheck(builder.Configuration.GetConnectionString("ProductContext")),
        HealthStatus.Unhealthy)
    .AddCheck<DaprHealthCheck>("dapr-check");


builder.Services.AddGraphQLServer().AddQueryType<ProductGraphQuery>().AddProjections().AddFiltering().AddSorting();

var app = builder.Build();

app.ConfigureBaseApplicationBuilders();
app.ConfigureBaseEndpointBuilders();

// Configure the Minimal APIs
//app.MapProduct().RequireAuthorization("ApiScope");
app.MapProduct();

//Initialize DB
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
services.GetRequiredService<DbInitializer>().Run();

// Capture metrics about all received HTTP requests.
//app.UseHttpMetrics();
//app.MapMetrics();

app.MapGraphQL();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }