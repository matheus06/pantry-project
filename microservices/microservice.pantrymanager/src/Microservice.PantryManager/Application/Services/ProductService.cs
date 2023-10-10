using Microservice.PantryManager.Application.CustomExceptions;
using Microservice.PantryManager.Domain;
using Microservice.PantryManager.Infra.Specifications;
using Platform.Infra.Database.Abstractions;

namespace Microservice.PantryManager.Application.Services;

public class ProductService
{
    private readonly IRepository<Product> _productRepository;
    
    public ProductService(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task CreateNewProduct(Guid id, string name, string description)
    {
        var product = await  _productRepository.GetAsync(new ProductSpecification { NamePattern = name });
        if (product != null)
            throw new ResourceWithNameFoundException(nameof(product), name);
        
        var newProduct = new Product(id, name, description);
        await _productRepository.AddAsync(newProduct);
    }
}