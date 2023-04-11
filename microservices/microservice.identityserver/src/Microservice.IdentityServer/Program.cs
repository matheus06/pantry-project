using Microservice.IdentityServer;
using Platform;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

var serviceName = builder.Configuration["MicroserviceSettings:Service"];

builder.Services.AddIdentityServer(options =>
    {
        // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
        options.EmitStaticAudienceClaim = true;
    })
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryClients(Config.Clients)
    .AddTestUsers(TestUsers.Users);

//Health Checks
builder.Services.AddHealthChecks();

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

app.UseIdentityServer();

app.ConfigureBaseApplicationBuilders();
app.ConfigureBaseEndpointBuilders();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }