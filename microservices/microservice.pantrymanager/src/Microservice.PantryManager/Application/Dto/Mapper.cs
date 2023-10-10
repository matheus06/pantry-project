using Microservice.PantryManager.Domain.PantryAggregate;
using Microservice.PantryManager.Domain.PantryAggregate.Entities;
using Microservice.PantryManager.Domain.PantryAggregate.Enums;
using Microservice.PantryManager.Domain.PantryAggregate.ValueObjects;

namespace Microservice.PantryManager.Application.Dto;

public static class Mapper
{
    public static PantryResponse MapToPantryResponseDto(Pantry pantry)
    {
        return new PantryResponse
        {
            Id = pantry.Id,
            Name = pantry.Name,
            Description = pantry.Description,
            Items = pantry.Items.Select( x=> new PantryItemsResponse
            {
                ProductId = x.ProductId,
                Quantity =MapToQuantityResponse(x.ItemQuantity) 
            })
        };
    }
    
    public static PantryOwnerResponse MapToPantryOwnerResponseDto(PantryOwner pantryOwner)
    {
        return new PantryOwnerResponse
        {
            Id = pantryOwner.Id,
            FirstName = pantryOwner.FirstName,
            LastName = pantryOwner.LastName,
            Email = pantryOwner.Email,
        };
    }
    
    public static QuantityDto MapToQuantityResponse(Quantity quantity)
    {
        return new QuantityDto
        {
            Amount = quantity.Amount,
            Unit = (VolumeUnitDto)quantity.Unit
        };
    }

    public static Quantity MapToQuantity(QuantityDto quantityRequest)
    {
        return new Quantity(quantityRequest.Amount,(VolumeUnit)quantityRequest.Unit);
    }
}