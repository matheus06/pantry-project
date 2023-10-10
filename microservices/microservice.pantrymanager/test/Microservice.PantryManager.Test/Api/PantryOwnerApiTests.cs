using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using Microservice.PantryManager.Application.Dto;
using Microservice.PantryManager.Infra.Database;
using Platform.Security;
using Platform.Tests;

namespace Microservice.PantryManager.Test.Api;

public class PantryOwnerApiTests : IClassFixture<CustomWebApplicationFactory<Program, PantryContext>>
{
    private readonly HttpClient _httpClient ;
    private string ApiToken { get; }   

    
    public PantryOwnerApiTests(CustomWebApplicationFactory<Program, PantryContext> factory)
    {
        _httpClient = factory.CreateClient();
        
        ApiToken = MockJwtTokens.GenerateJwtToken(new List<Claim> { new (PolicyNames.ServiceScopes, "pantry")});
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", ApiToken);
    }
    
    [Fact]
    public async Task PostPantryOwner_MissingFirstName_ShouldReturnBadRequest()
    {
        // Act
        var response = await _httpClient.PostAsJsonAsync("/pantryOwner", new PantryOwnerCreateRequest{ LastName = "Nunes", Email ="zico@goal.com" });
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PostPantryOwner_MissingLastName_ShouldReturnBadRequest()
    {
        // Act
        var response = await _httpClient.PostAsJsonAsync("/pantryOwner", new PantryOwnerCreateRequest{ FirstName = "Zico", Email ="zico@goal.com" });
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PostPantryOwner_MissingEmail_ShouldReturnBadRequest()
    {
        // Act
        var response = await _httpClient.PostAsJsonAsync("/pantryOwner", new PantryOwnerCreateRequest{ FirstName = "Zico", LastName = "Nunes"});
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PostPantryOwner_MissingDescription_ShouldReturnBadRequest()
    {
        // Act
        var response = await _httpClient.PostAsJsonAsync("/pantryOwner", new PantryOwnerCreateRequest{ FirstName = "Zico", LastName = "Nunes", Email ="zico@goal.com" });

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}