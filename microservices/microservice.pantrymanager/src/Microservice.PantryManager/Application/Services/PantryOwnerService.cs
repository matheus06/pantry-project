using Microservice.PantryManager.Application.Dto;
using Microservice.PantryManager.Domain;
using Microservice.PantryManager.Domain.PantryAggregate.Entities;
using Microservice.PantryManager.Infra.Specifications;
using Platform.Infra.Database.Abstractions;

namespace Microservice.PantryManager.Application.Services;

public class PantryOwnerService
{
    private readonly IRepository<PantryOwner> _pantryOwnerRepository;

    public PantryOwnerService(IRepository<PantryOwner> pantryOwnerRepository)
    {
        _pantryOwnerRepository = pantryOwnerRepository;
    }

    public async Task<PantryOwnerResponse> CreatePantryOwner(PantryOwnerCreateRequest pantryOwnerCreateRequest)
    {
        var pantry = await _pantryOwnerRepository.GetAsync(new PantryOwnerSpecification
        {
            FirstNamePattern = pantryOwnerCreateRequest.FirstName,
            LastNamePattern = pantryOwnerCreateRequest.LastName,
            EmailPattern = pantryOwnerCreateRequest.Email
        });
        if (pantry != null)
            throw new Exception($"PantryOwner with name: {pantryOwnerCreateRequest.FirstName} and email: {pantryOwnerCreateRequest.Email} already exist");
        
        var newPantryOwner = new PantryOwner(pantryOwnerCreateRequest.FirstName, pantryOwnerCreateRequest.LastName, pantryOwnerCreateRequest.Email);
        await _pantryOwnerRepository.AddAsync(newPantryOwner);

        var pantryOwnerResponse = Mapper.MapToPantryOwnerResponseDto(newPantryOwner);
        return pantryOwnerResponse;
    }
}