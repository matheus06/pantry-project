using System.Reflection;
using Microservice.RecipeManager.Api;
using Microservice.RecipeManager.Infra;
using Microservice.RecipeManager.Infra.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Platform;
using Platform.Infra.Database;
using Platform.Infra.Messaging;
using Prometheus;

var builder = WebApiApplicationBuilder.Build<Program>(args);

//SQL Server
builder.Services.AddDbContext<RecipeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RecipeContext")));

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
        "database-recipe-check",
        new SqlConnectionHealthCheck(builder.Configuration.GetConnectionString("RecipeContext")),
        HealthStatus.Unhealthy)
    .AddCheck<DaprHealthCheck>("dapr-check");

var app = builder.Build();

app.ConfigureBaseApplicationBuilders();
app.ConfigureBaseEndpointBuilders();

// Configure the Minimal APIs
app.MapRecipe().RequireAuthorization("ApiScope");
app.MapSubscription();

// Dapr will send serialized event object vs. being raw CloudEvent
app.UseCloudEvents();
// needed for Dapr pub/sub routing
app.MapSubscribeHandler();

//Initialize DB
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
services.GetRequiredService<DbInitializer>().Run();

// Capture metrics about all received HTTP requests.
app.UseHttpMetrics();
app.MapMetrics();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }