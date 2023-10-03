using Hangfire;
using Hangfire.SqlServer;
using Microservice.Scheduler.Api;
using Microservice.Scheduler.Infra;
using Microservice.Scheduler.Infra.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Platform;
using Platform.Infra.Database;
using Platform.Infra.Messaging;

var builder = WebApiApplicationBuilder.Build<Program>(args);


//SQL Server
builder.Services.AddDbContext<SchedulerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchedulerContext")));

builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("SchedulerContext"), new SqlServerStorageOptions 
{
    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
    QueuePollInterval = TimeSpan.Zero,
    UseRecommendedIsolationLevel = true,
    DisableGlobalLocks = true,
    PrepareSchemaIfNecessary = false
}));

builder.Services.AddHangfireServer();

//Dapr for Bus
builder.Services.AddDaprClient();

//Add IoC
builder.Services.AddIoC();

//Health Checks
builder.Services.AddHealthChecks()
    // Add a health check for a SQL Server database
    .AddCheck(
        "database-scheduler-check",
        new SqlConnectionHealthCheck(builder.Configuration.GetConnectionString("SchedulerContext")),
        HealthStatus.Unhealthy)
    .AddCheck<DaprHealthCheck>("dapr-check");

var app = builder.Build();

app.ConfigureBaseApplicationBuilders();
app.ConfigureBaseEndpointBuilders();

// Configure the Minimal APIs
app.MapScheduler();

app.UseHangfireDashboard( options: new DashboardOptions{
    Authorization = new[] { new HangFireDashboardAuthorizationFilter() }
});


//Initialize DB
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
services.GetRequiredService<DbInitializer>().Run();

// Capture metrics about all received HTTP requests.
//app.UseHttpMetrics();
//app.MapMetrics();

app.Run();
