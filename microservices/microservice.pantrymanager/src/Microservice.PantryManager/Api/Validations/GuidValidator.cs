using FluentValidation;

namespace Microservice.PantryManager.Api.Validations;

public  class GuidValidator : AbstractValidator<Guid>
{
    public  GuidValidator()
    {
        RuleFor(value => value).NotEmpty().WithMessage("Id Should Not Be Empty");
    }
}