using FluentValidation;
using Microservice.PantryManager.Application.Dto;

namespace Microservice.PantryManager.Api.Validations;

public  class PantryOwnerCreateRequestValidator : AbstractValidator<PantryOwnerCreateRequest>
{
    public  PantryOwnerCreateRequestValidator()
    {
        RuleFor(pantryOwnerCreateRequest => pantryOwnerCreateRequest.FirstName).NotEmpty().Length(0, 50);
        RuleFor(pantryOwnerCreateRequest => pantryOwnerCreateRequest.LastName).NotEmpty().Length(0, 50);
        RuleFor(pantryOwnerCreateRequest => pantryOwnerCreateRequest.Email).NotEmpty().Length(0, 50);
    }
}