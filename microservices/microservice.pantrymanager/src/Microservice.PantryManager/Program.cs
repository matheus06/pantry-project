using System.Reflection;
using FluentValidation;
using Microservice.PantryManager.Api;
using Microservice.PantryManager.Api.Validations;
using Microservice.PantryManager.Infra;
using Microservice.PantryManager.Infra.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Platform;
using Platform.Infra.Database;
using Platform.Infra.Messaging;
using Prometheus;

var builder = WebApiApplicationBuilder.Build<Program>(args);

//SQL Server
builder.Services.AddDbContext<PantryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PantryContext")));

//MediatR for Domain Events 
builder.Services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

//Dapr for Bus
builder.Services.AddDaprClient();

//Add IoC
builder.Services.AddIoC();

//Health Checks
builder.Services.AddHealthChecks()
    // Add a health check for a SQL Server database
    .AddCheck(
        "database-pantry-check",
        new SqlConnectionHealthCheck(builder.Configuration.GetConnectionString("PantryContext")),
        HealthStatus.Unhealthy)
    .AddCheck<DaprHealthCheck>("dapr-check")
    // Report health check results in the metrics output.
    .ForwardToPrometheus();

//Health Check UI
builder.Services.AddHealthChecksUI().AddInMemoryStorage();

builder.Services.AddValidatorsFromAssemblyContaining<PantryRequestValidator>();

var app = builder.Build();

app.ConfigureBaseApplicationBuilders();
app.ConfigureBaseEndpointBuilders();

//Health Check UI
app.UseHealthChecksUI(config => config.UIPath = "/hc-ui");

// Configure the Minimal APIs
app.MapPantry().RequireAuthorization("ApiScope");
app.MapPantryOwner().RequireAuthorization("ApiScope");
app.MapDaprSubscription();

// Dapr will send serialized event object vs. being raw CloudEvent
app.UseCloudEvents();
// needed for Dapr pub/sub routing
app.MapSubscribeHandler();

//Initialize DB
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
services.GetRequiredService<DbInitializer>().Run();

// Capture metrics about all received HTTP requests.
//app.UseHttpMetrics();
//app.MapMetrics();

// Error Handling
app.UseExceptionHandler("/error");
app.MapError();

//Auths
app.UseAuthentication();
app.UseAuthorization();



// Capture metrics about all received HTTP requests.
//app.UseHttpMetrics();
//app.MapMetrics();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }