using Microsoft.Extensions.Diagnostics.HealthChecks;
using Dapr.Client;

namespace Platform.Infra.Messaging;

public class DaprHealthCheck : IHealthCheck
{
    private readonly DaprClient _daprClient;

    public DaprHealthCheck(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return await _daprClient.CheckHealthAsync(cancellationToken)
            ? new HealthCheckResult(HealthStatus.Healthy, "Dapr Sidecar is healthy")
            : new HealthCheckResult(HealthStatus.Unhealthy, "Dapr Sidecar is unhealthy");
    }
}