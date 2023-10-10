using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using Microservice.PantryManager.Application.Dto;
using Microservice.PantryManager.Infra.Database;
using Platform.Security;
using Platform.Tests;

namespace Microservice.PantryManager.Test.Api;

public class PantryApiTests : IClassFixture<CustomWebApplicationFactory<Program, PantryContext>>
{
    private readonly HttpClient _httpClient ;
    private const string PantryApiUri = "/pantry";
    private string ApiToken { get; }    
    
    public PantryApiTests(CustomWebApplicationFactory<Program,PantryContext> factory)
    {
        _httpClient = factory.CreateClient();
        ApiToken = MockJwtTokens.GenerateJwtToken(new List<Claim> { new (PolicyNames.ServiceScopes, "pantry")});
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", ApiToken);
    }
    
    [Fact]
    public async Task PostPantry_MissingDescription_ShouldReturnBadRequest()
    {
        // Act
        
        var response = await _httpClient.PostAsJsonAsync(PantryApiUri, new PantryCreateRequest{ OwnerId = Guid.NewGuid(), Name = "Name"});
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PostPantry_MissingName_ShouldReturnBadRequest()
    {
        // Act
        var response = await _httpClient.PostAsJsonAsync(PantryApiUri, new PantryCreateRequest{ OwnerId = Guid.NewGuid(), Description = "Description"});
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PostPantry_MissingOwnerId_ShouldReturnBadRequest()
    {
        // Act
        var response = await _httpClient.PostAsJsonAsync(PantryApiUri, new PantryCreateRequest{ Name = "Name",  Description = "Description"});
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PostPantry_MissingDescription_ResponseContentShouldContainCorrectText()
    {
        // Act
        var response = await _httpClient.PostAsJsonAsync(PantryApiUri, new PantryCreateRequest{ OwnerId = Guid.NewGuid(), Name = "Name"});
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("'Description' must not be empty", content);
    }
    
    [Fact]
    public async Task PostPantry_MissingName_ResponseContentShouldContainCorrectText()
    {
        // Act
        var response = await _httpClient.PostAsJsonAsync(PantryApiUri, new PantryCreateRequest{ OwnerId = Guid.NewGuid(), Description = "Description"});
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("'Name' must not be empty", content);
    }
    
    [Fact]
    public async Task PostPantry_MissingOwnerId_ResponseContentShouldContainCorrectText()
    {
        // Act
        var response = await _httpClient.PostAsJsonAsync(PantryApiUri, new PantryCreateRequest{ Name = "Name", Description = "Description"});
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("'Owner Id' must not be empty", content);
    }
    
    
    [Fact]
    public async Task PostPantry_MissingNameAndDescription_ResponseContentShouldContainCorrectText()
    {
        // Act
        var response = await _httpClient.PostAsJsonAsync(PantryApiUri, new PantryCreateRequest{ OwnerId = Guid.NewGuid()});
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("'Name' must not be empty", content);
        Assert.Contains("'Description' must not be empty", content);
    }
    
    [Fact]
    public async Task PostPantry_NonExistingOwnerId_ShouldReturnNotFound()
    {
        // Act
        var response = await _httpClient.PostAsJsonAsync(PantryApiUri, new PantryCreateRequest{ OwnerId = Guid.NewGuid(),Name = "Name", Description = "Description"});
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task PostPantry_NonExistingOwnerId_ResponseContentShouldContainCorrectText()
    {
        // Act
        var response = await _httpClient.PostAsJsonAsync(PantryApiUri, new PantryCreateRequest{ OwnerId = Guid.NewGuid(),Name = "Name", Description = "Description"});
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("'pantryOwner' was not found", content);
    }
    
    [Fact]
    public async Task PostPantry_ExistingOwnerId_ShouldReturnCreated()
    {
        // Arrange
        var pantryOwnerResponse = await _httpClient.PostAsJsonAsync("/pantryOwner", new PantryOwnerCreateRequest{ FirstName = "Zico", LastName = "Nunes", Email ="zico@goal.com" });
        var pantryOwner = await pantryOwnerResponse.Content.ReadFromJsonAsync<PantryOwnerResponse>();

        // Act
        var response = await _httpClient.PostAsJsonAsync(PantryApiUri, new PantryCreateRequest{ OwnerId = pantryOwner.Id, Name = "Name", Description = "Description"});
        
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
    
    
    [Fact]
    public async Task GetPantry_NonExistingId_ShouldReturnNotFound()
    {
        // Act
        var response = await _httpClient.GetAsync($"{PantryApiUri}/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task GetPantry_MissingId_ShouldReturnBadRequest()
    {
        // Act
        var response = await _httpClient.GetAsync($"{PantryApiUri}/{Guid.Empty}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task GetPantry_MissingId_ResponseContentShouldContainCorrectText()
    {
        // Act
        var response = await _httpClient.GetAsync($"{PantryApiUri}/{Guid.Empty}");

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Id Should Not Be Empty", content);
    }
    
    [Fact]
    public async Task GetPantry_ExistingId_ShouldReturnOK()
    {
        // Arrange
        var pantryOwnerResponse = await _httpClient.PostAsJsonAsync("/pantryOwner", new PantryOwnerCreateRequest{ FirstName = "Andrade", LastName = "Nunes", Email ="andrade@goal.com" });
        var createdPantryOwner = await pantryOwnerResponse.Content.ReadFromJsonAsync<PantryOwnerResponse>();
        var pantryResponse = await _httpClient.PostAsJsonAsync(PantryApiUri, new PantryCreateRequest{ OwnerId = createdPantryOwner.Id, Name = "Name", Description = "Description"});
        var createdPantry = await pantryResponse.Content.ReadFromJsonAsync<PantryResponse>();
       
        // Act
       var response = await _httpClient.GetAsync($"{PantryApiUri}/{createdPantry.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}