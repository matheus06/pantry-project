using FluentValidation;
using Microservice.ProductManager.Application.Commands.ProductCommands;

namespace Microservice.ProductManager.Api.Validations
{
    public  class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public  CreateProductCommandValidator()
        {
            RuleFor(createProductCommand => createProductCommand.Name).NotEmpty().Length(0, 50);
            RuleFor(createProductCommand => createProductCommand.Description).NotEmpty().Length(0, 100);
        }
    }
}