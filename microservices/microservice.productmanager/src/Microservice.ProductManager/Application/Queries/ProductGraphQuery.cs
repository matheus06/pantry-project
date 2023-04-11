using Microservice.ProductManager.Application.Dto;
using Microservice.ProductManager.Domain;
using Microservice.ProductManager.Infra.Specifications;
using Platform.Infra.Database.Abstractions;

namespace Microservice.ProductManager.Application.Queries;

public class ProductGraphQuery
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<List<ProductResponse>> GetProducts([Service] IRepository<Product> repository)
    {
        var products = await repository.ListAsync(new ProductSpecification());
        return products.Select(Mapper.MapToProductResponse).ToList();
    }
}