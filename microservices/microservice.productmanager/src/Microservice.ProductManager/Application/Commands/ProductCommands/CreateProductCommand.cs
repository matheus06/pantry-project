using FluentResults;
using MediatR;
using Microservice.ProductManager.Application.Dto;
using Microservice.ProductManager.Domain;
using Microservice.ProductManager.Infra.Specifications;
using Platform.ErrorHandling.ApplicationErrors;
using Platform.Infra.Database.Abstractions;

namespace Microservice.ProductManager.Application.Commands.ProductCommands;

public class CreateProductCommand : IRequest<FluentResults.Result<ProductResponse>>
{
    public string Name { get;  private set; }
    public string Description { get; private set; }
    
    public CreateProductCommand(string name,string description)
    {
        Name = name;
        Description = description;
    }
}

public class CreateRecipeCommandHandler : IRequestHandler<CreateProductCommand, FluentResults.Result<ProductResponse>>
{
    private readonly IRepository<Product> _productRepository;
    
    public CreateRecipeCommandHandler(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<FluentResults.Result<ProductResponse>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var existingProduct = await _productRepository.GetAsync(new ProductSpecificationByName { NamePattern = command.Name}, cancellationToken);
        if (existingProduct != null)
          return  Result.Fail(new ConflictNameError("Product", command.Name));
        
        var product = Mapper.MapToProduct(command);
        await _productRepository.AddAsync(product, cancellationToken);
        
        return Result.Ok(Mapper.MapToProductResponse(product));
    }
}