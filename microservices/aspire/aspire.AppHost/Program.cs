using k8s.Models;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddDapr();

builder.AddProject<Projects.Microservice_Pantry>("microservice.pantry")
    .WithDaprSidecar("daprpantrymanager");

builder.AddProject<Projects.Microservice_Product>("microservice.product")
    .WithDaprSidecar("daprproductmanager");

builder.AddProject<Projects.Microservice_Recipe>("microservice.recipe")
    .WithDaprSidecar("daprrecipemanager");

builder.AddProject<Projects.Microservice_Scheduler>("microservice.scheduler")
        .WithDaprSidecar("daprscheduler");

builder.AddProject<Projects.Microservice_IdentityServer>("microservice.identityserver");

builder.AddContainer("ui", "ui-pantry")
    .WithServiceBinding(containerPort: 80, hostPort: 4200, name: "endpoint");


builder.Build().Run();
