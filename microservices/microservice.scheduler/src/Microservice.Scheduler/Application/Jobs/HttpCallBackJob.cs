using Microservice.Scheduler.Application.Dto;
using Microservice.Scheduler.Application.Services;

namespace Microservice.Scheduler.Application.Jobs;

public class HttpCallBackJob
{
    private readonly IHttpClientFactory  _httpClientFactory ;
    private readonly ILogger<HttpCallBackJob> _logger;

    public HttpCallBackJob(IHttpClientFactory  httpClientFactory , ILogger<HttpCallBackJob> logger)
    {
        _httpClientFactory  = httpClientFactory;
        _logger = logger;
    }
    
    public void PublishMessage(HttpCallBackJobRequest jobRequest)
    {
        var client = _httpClientFactory.CreateClient();
        _logger.LogInformation("Posting RecurrentJobTriggeredIntegrationEvent");
        client.PostAsJsonAsync(jobRequest.Uri, jobRequest.Payload);
        _logger.LogInformation("Published");
    }
}
