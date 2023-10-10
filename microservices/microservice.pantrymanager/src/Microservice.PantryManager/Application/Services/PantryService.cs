using Microservice.PantryManager.Application.CustomExceptions;
using Microservice.PantryManager.Application.Dto;
using Microservice.PantryManager.Domain;
using Microservice.PantryManager.Domain.PantryAggregate;
using Microservice.PantryManager.Domain.PantryAggregate.Entities;
using Microservice.PantryManager.Infra.Specifications;
using Platform.Infra.Database.Abstractions;

namespace Microservice.PantryManager.Application.Services;

public class PantryService
{
    private readonly IRepository<Pantry> _pantryRepository;
    private readonly IRepository<PantryOwner> _pantryOwnerRepository;
    private readonly IRepository<PantryItem> _pantryItemRepository;
    private readonly IRepository<Product> _productRepository;

    public PantryService(IRepository<Pantry> pantryRepository, IRepository<PantryOwner> pantryOwnerRepository, IRepository<PantryItem> pantryItemRepository, IRepository<Product> productRepository)
    {
        _pantryRepository = pantryRepository;
        _pantryOwnerRepository = pantryOwnerRepository;
        _pantryItemRepository = pantryItemRepository;
        _productRepository = productRepository;
    }
    
    public async  Task<PantryResponse> GetPantry(Guid pantryId)
    {
        var pantry = await  _pantryRepository.GetByIdAsync(pantryId);
        if (pantry == null)
            throw new ResourceNotFoundException(nameof(pantry));
        
        var pantryDto = Mapper.MapToPantryResponseDto(pantry);
        return pantryDto;
    }

    public async  Task<PantryResponse> CreatePantry(PantryCreateRequest pantryCreateRequest)
    {
        var pantryOwner = await _pantryOwnerRepository.GetByIdAsync(pantryCreateRequest.OwnerId);
        
        if (pantryOwner == null)
            throw new ResourceNotFoundException(nameof(pantryOwner));
        
        var pantry = await  _pantryRepository.GetAsync(new PantrySpecification { NamePattern = pantryCreateRequest.Name, PantryOwnerId = pantryOwner.Id});
        if (pantry != null)
            throw new ResourceWithNameFoundException(nameof(pantry), pantryCreateRequest.Name);
        
        var newPantry = new Pantry(pantryOwner.Id, pantryCreateRequest.Name, pantryCreateRequest.Description);
        await _pantryRepository.AddAsync(newPantry);

        var pantryDto = Mapper.MapToPantryResponseDto(newPantry);
        return pantryDto;
    }
    
    public async  Task<PantryResponse> UpdatePantry(PantryUpdateRequest pantryUpdateRequest)
    {
        var pantry = await  _pantryRepository.GetByIdAsync(pantryUpdateRequest.Id);
        if (pantry == null)
            throw new ResourceNotFoundException(nameof(pantry));
        
        pantry.Updated(pantryUpdateRequest.Name, pantryUpdateRequest.Description);
        await _pantryRepository.UpdateAsync(pantry);

        var pantryDto = Mapper.MapToPantryResponseDto(pantry);
        return pantryDto;
    }

    public async Task<PantryResponse> AddPantryItem(Guid pantryId, AddPantryItemRequest addPantryItemRequest)
    {
        var pantry = await  _pantryRepository.GetByIdAsync(pantryId);
        if (pantry == null)
            throw new ResourceNotFoundException(nameof(pantry));
        
        var product = await  _productRepository.GetByIdAsync(addPantryItemRequest.ProductId);
        if (product == null)
            throw new ResourceNotFoundException(nameof(product));
        
        var quantity = Mapper.MapToQuantity(addPantryItemRequest.Quantity);
        var pantryItem = new PantryItem(pantryId, addPantryItemRequest.ProductId, quantity);
        pantry.AddItem(pantryItem);
        
        await _pantryRepository.UpdateAsync(pantry);
        
        var pantryDto = Mapper.MapToPantryResponseDto(pantry);
        return pantryDto;
    }

    public async Task<PantryResponse> DeletePantryItem(Guid pantryId, Guid productId)
    {
        var pantry = await  _pantryRepository.GetByIdAsync(pantryId);
        if (pantry == null)
            throw new ResourceNotFoundException(nameof(pantry));
        
        pantry.RemoveItem(productId);
        
        await _pantryRepository.UpdateAsync(pantry);
        
        var pantryDto = Mapper.MapToPantryResponseDto(pantry);
        return pantryDto;
    }
}
