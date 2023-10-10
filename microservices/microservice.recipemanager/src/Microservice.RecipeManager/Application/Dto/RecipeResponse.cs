namespace Microservice.RecipeManager.Application.Dto;

public class RecipeResponse
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string Instructions { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
}