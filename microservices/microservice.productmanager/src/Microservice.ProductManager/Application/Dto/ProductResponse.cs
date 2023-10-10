namespace Microservice.ProductManager.Application.Dto;

public class ProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}