namespace Microservice.PantryManager.Application.Dto;

public class PantryCreateRequest
{
    public Guid OwnerId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}