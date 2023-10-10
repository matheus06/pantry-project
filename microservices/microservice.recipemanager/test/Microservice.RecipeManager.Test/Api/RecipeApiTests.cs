using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microservice.RecipeManager.Infra.Database;
using Platform.Security;
using Platform.Tests;

namespace Microservice.RecipeManager.Test.Api
{
    public class RecipeApiTests : IClassFixture<CustomWebApplicationFactory<Program, RecipeContext>>
    {
        private readonly HttpClient _httpClient;
        private const string RecipeApiUri = "/recipe";
        private string ApiToken { get; }    
    
        public RecipeApiTests(CustomWebApplicationFactory<Program,RecipeContext> factory)
        {
            _httpClient = factory.CreateClient();
        
            ApiToken = MockJwtTokens.GenerateJwtToken(new List<Claim> { new (PolicyNames.ServiceScopes, "recipe")});
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", ApiToken);
        }
    
        [Fact]
        public async Task GetRecipe_WhenRecipeDoesNotExist_ShouldReturnNotFound()
        {
            // Act
            var response = await _httpClient.GetAsync($"{RecipeApiUri}/{Guid.NewGuid()}");
        
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    
    }
}