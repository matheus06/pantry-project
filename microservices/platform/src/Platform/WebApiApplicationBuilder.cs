using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Platform.Security;
using Prometheus;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Platform;

public static class WebApiApplicationBuilder
{
    public static WebApplicationBuilder Build<TStartup>(string[] args) where TStartup : class
    {
        var builder = WebApplication.CreateBuilder(args);
     
        var serviceName = builder.Configuration["MicroserviceSettings:Service"];
        
        //Serilog and ElasticSearch
        var elasticSearchUri = builder.Configuration["ElasticConfiguration:Uri"];
        builder.Host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.
                WriteTo.Async(writeTo =>
                    writeTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss,fff} [{ThreadId}] {Level:u4} {Message:lj}{NewLine}{Exception}"))
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
        
        //Configure IdentityServer
        var identityServerUri = builder.Configuration["IdentityServer:Uri"];
        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = identityServerUri;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidIssuer = "https://localdev-tls.me"
                };
                options.RequireHttpsMetadata = false;
            });

        //Configure Authorization
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(PolicyNames.ServiceScopes, serviceName ?? $"{Guid.NewGuid()}");
            });
        });
        
        // Configure Open API
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(o => 
        { 
            o.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            o.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
            o.DocumentFilter<PathPrefixInsertDocumentFilter>($"/api-{serviceName}");
        });
        
        return builder;
    }


    public static void ConfigureBaseApplicationBuilders(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseSerilogRequestLogging();
    }
    
    public static void ConfigureBaseEndpointBuilders(this IEndpointRouteBuilder app)
    {
        //Health Check Api
        app.MapHealthChecks("/healthz", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        
    }
    
    

    // Capture metrics about all received HTTP requests.

  
    
}
