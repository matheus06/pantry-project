using MediatR;
using Microservice.RecipeManager.Application.Dto;

namespace Microservice.RecipeManager.Application.Commands.Ingredient;

public class CreateIngredientCommand : IRequest<RecipeResponse>
{
    
}

public class CreateRecipeCommandHandler : IRequestHandler<CreateIngredientCommand, RecipeResponse>
{
    public Task<RecipeResponse> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}