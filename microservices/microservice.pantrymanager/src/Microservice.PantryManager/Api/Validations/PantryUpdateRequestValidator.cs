using FluentValidation;
using Microservice.PantryManager.Application.Dto;

namespace Microservice.PantryManager.Api.Validations;

public  class PantryUpdateRequestValidator : AbstractValidator<PantryUpdateRequest>
{
    public  PantryUpdateRequestValidator()
    {
        RuleFor(pantryRequest => pantryRequest.Name).NotEmpty().Length(0, 50);
        RuleFor(pantryRequest => pantryRequest.Description).NotEmpty().Length(0, 100);
        RuleFor(pantryRequest => pantryRequest.Id).NotEmpty();
    }
}