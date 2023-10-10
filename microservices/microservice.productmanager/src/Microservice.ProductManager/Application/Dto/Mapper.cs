using Microservice.ProductManager.Application.Commands.ProductCommands;
using Microservice.ProductManager.Domain;

namespace Microservice.ProductManager.Application.Dto;

public static class Mapper
{
    public static ProductResponse MapToProductResponse(Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description
        };
    }
    
    public static CreateProductCommand MapToProductCommand (ProductRequest productRequest)
    {
        return new CreateProductCommand(productRequest.Name, productRequest.Description);
    }
    
    public static Product MapToProduct(CreateProductCommand command)
    {
        return new Product(command.Name, command.Description);
    }
}