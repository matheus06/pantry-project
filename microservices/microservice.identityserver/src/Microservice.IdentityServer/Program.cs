using Microservice.IdentityServer;
using Microservice.IdentityServer.Infra;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Platform;
using Platform.Infra.Database;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

var serviceName = builder.Configuration["MicroserviceSettings:Service"];

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        options.RequireHeaderSymmetry = false;
        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
});

var migrationsAssembly = typeof(Program).Assembly.GetName().Name;
builder.Services.AddIdentityServer()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b => b.UseSqlServer(builder.Configuration.GetConnectionString("IdentityServerContext"),
            sql => sql.MigrationsAssembly(migrationsAssembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b => b.UseSqlServer(builder.Configuration.GetConnectionString("IdentityServerContext"),
            sql => sql.MigrationsAssembly(migrationsAssembly));
    })
    .AddTestUsers(TestUsers.Users);

builder.Services.AddRazorPages();

//Add IoC
builder.Services.AddIoC();

//Health Checks
builder.Services.AddHealthChecks()
    // Add a health check for a SQL Server database
    .AddCheck(
        "database-identity-check",
        new SqlConnectionHealthCheck(builder.Configuration.GetConnectionString("IdentityServerContext")),
        HealthStatus.Unhealthy);

//Serilog and ElasticSearch
var elasticSearchUri = builder.Configuration["ElasticConfiguration:Uri"];
builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.WriteTo.Async(writeTo =>
            writeTo.Console(
                outputTemplate:
                "{Timestamp:yyyy-MM-dd HH:mm:ss,fff} [{ThreadId}] {Level:u4} {Message:lj}{NewLine}{Exception}"))
        .Enrich.WithExceptionDetails()
        .Enrich.WithThreadId();
    
    if (!string.IsNullOrEmpty(elasticSearchUri))
    {
        loggerConfiguration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchUri))
        {
            IndexFormat = $"{serviceName?.ToLower()}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".","-")}-{DateTime.UtcNow:yyyy-MM}"
        });
    }
});


var app = builder.Build();

app.UseForwardedHeaders();

app.UseIdentityServer();

if (app.Environment.IsDevelopment())
{
    app.UseCookiePolicy(new CookiePolicyOptions()
    {
        MinimumSameSitePolicy = SameSiteMode.Lax
    });
}
else
{
    app.UseCookiePolicy(new CookiePolicyOptions()
    {
        MinimumSameSitePolicy = SameSiteMode.Strict
    });
}

app.ConfigureBaseEndpointBuilders();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages().RequireAuthorization();

//Initialize DB
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
services.GetRequiredService<DbInitializer>().Run();

app.InitializeDatabase();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }