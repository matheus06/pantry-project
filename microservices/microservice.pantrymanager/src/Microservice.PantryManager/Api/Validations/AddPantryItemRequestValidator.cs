using FluentValidation;
using Microservice.PantryManager.Application.Dto;

namespace Microservice.PantryManager.Api.Validations;

public  class AddPantryItemRequestValidator : AbstractValidator<AddPantryItemRequest>
{
    public  AddPantryItemRequestValidator()
    {
        RuleFor(addPantryItemRequest => addPantryItemRequest.ProductId).NotEmpty();
        RuleFor(addPantryItemRequest => addPantryItemRequest.Quantity.Amount).NotEmpty().GreaterThan(0).LessThanOrEqualTo(10);
        RuleFor(addPantryItemRequest => addPantryItemRequest.Quantity.Unit).NotEmpty();
    }
}