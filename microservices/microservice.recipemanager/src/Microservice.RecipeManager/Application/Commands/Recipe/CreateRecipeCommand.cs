using MediatR;
using Microservice.RecipeManager.Application.Dto;

namespace Microservice.RecipeManager.Application.Commands.Recipe;

public class CreateRecipeCommand : IRequest<RecipeResponse>
{
    
}

public class CreateRecipeCommandHandler : IRequestHandler<CreateRecipeCommand, RecipeResponse>
{
    public Task<RecipeResponse> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}