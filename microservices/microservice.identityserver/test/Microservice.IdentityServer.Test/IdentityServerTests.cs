using System.Net;
using IdentityModel.Client;
using Microservice.IdentityServer.Infra.Database;
using Platform.Tests;

namespace Microservice.IdentityServer.Test;

public class IdentityServerTests : IClassFixture<CustomWebApplicationFactory<Program, IdentityServerContext>>
{
    private readonly CustomWebApplicationFactory<Program,IdentityServerContext> _factory;
    
    public IdentityServerTests(CustomWebApplicationFactory<Program,IdentityServerContext> factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task GetDiscoveryDocumentAsync_ShouldReturnNoError()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetDiscoveryDocumentAsync();
        
        // Assert
        Assert.False(response.IsError);
    }
        
    [Theory]
    [InlineData("pantry")]
    [InlineData("product")]
    [InlineData("recipe")]
    public async Task RequestClientCredentialsTokenAsync_ValidRequest_ShouldReturnOK(string requiredScope)
    {
        // Arrange
        var client = _factory.CreateClient();
        var discoveryResponse = await client.GetDiscoveryDocumentAsync();
        
        // Act
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = discoveryResponse.TokenEndpoint,
            ClientId = "test-client",
            ClientSecret = "test-secret",
            Scope = requiredScope
        });

        // Assert
        Assert.False(tokenResponse.IsError);
        Assert.Equal(HttpStatusCode.OK, tokenResponse.HttpResponse.StatusCode);
    }
    
    [Theory]
    [InlineData("pantry")]
    [InlineData("product")]
    [InlineData("recipe")]
    public async Task RequestClientCredentialsTokenAsync_ValidRequest_ShouldReturnTokenWithRequestedScope(string requiredScope)
    {
        // Arrange
        var client = _factory.CreateClient();
        var discoveryResponse = await client.GetDiscoveryDocumentAsync();
        
        // Act
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = discoveryResponse.TokenEndpoint,
            ClientId = "test-client",
            ClientSecret = "test-secret",
            Scope = requiredScope
        });

        // Assert
        Assert.False(tokenResponse.IsError);
        Assert.Equal(requiredScope, tokenResponse.Scope);
    }
    
    [Fact]
    public async Task RequestClientCredentialsTokenAsync_InvalidScope_ShouldReturnTokenError()
    {
        // Arrange
        var client = _factory.CreateClient();
        var discoveryResponse = await client.GetDiscoveryDocumentAsync();
        
        // Act
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = discoveryResponse.TokenEndpoint,
            ClientId = "test-client",
            ClientSecret = "test-secret",
            Scope = "invalidScope"
        });

        // Assert
        Assert.True(tokenResponse.IsError);
    }
    
    [Fact]
    public async Task RequestClientCredentialsTokenAsync_InvalidScope_ShouldReturnTokenBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var discoveryResponse = await client.GetDiscoveryDocumentAsync();
        
        // Act
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = discoveryResponse.TokenEndpoint,
            ClientId = "test-client",
            ClientSecret = "test-secret",
            Scope = "invalidScope"
        });

        // Assert
        Assert.True(tokenResponse.IsError);
        Assert.Equal(HttpStatusCode.BadRequest, tokenResponse.HttpResponse.StatusCode);
    }
    
    [Theory]
    [InlineData("pantry")]
    [InlineData("product")]
    [InlineData("recipe")]
    public async Task RequestClientCredentialsTokenAsync_InvalidClientId_ShouldReturnTokenBadRequest(string requiredScope)
    {
        // Arrange
        var client = _factory.CreateClient();
        var discoveryResponse = await client.GetDiscoveryDocumentAsync();
        
        // Act
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = discoveryResponse.TokenEndpoint,
            ClientId = "invalid-client",
            ClientSecret = "test-secret",
            Scope = requiredScope
        });

        // Assert
        Assert.True(tokenResponse.IsError);
        Assert.Equal(HttpStatusCode.BadRequest, tokenResponse.HttpResponse.StatusCode);
    }
    
    [Theory]
    [InlineData("pantry")]
    [InlineData("product")]
    [InlineData("recipe")]
    public async Task RequestClientCredentialsTokenAsync_InvalidSecret_ShouldReturnTokenBadRequest(string requiredScope)
    {
        // Arrange
        var client = _factory.CreateClient();
        var discoveryResponse = await client.GetDiscoveryDocumentAsync();
        
        // Act
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = discoveryResponse.TokenEndpoint,
            ClientId = "test-client",
            ClientSecret = "invalid-secret",
            Scope = requiredScope
        });

        // Assert
        Assert.True(tokenResponse.IsError);
        Assert.Equal(HttpStatusCode.BadRequest, tokenResponse.HttpResponse.StatusCode);
    }
}