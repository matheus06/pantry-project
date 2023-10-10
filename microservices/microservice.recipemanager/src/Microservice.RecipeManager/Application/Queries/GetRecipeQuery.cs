using FluentResults;
using MediatR;
using Microservice.RecipeManager.Application.Dto;
using Microservice.RecipeManager.Application.ErrorHandling.ApplicationErrors;
using Microservice.RecipeManager.Domain.RecipeAggregate;
using Platform.Infra.Database.Abstractions;

namespace Microservice.RecipeManager.Application.Queries;

public class GetRecipeQuery : IRequest<Result<RecipeResponse>>
{
    public Guid Id { get;  private set; }

    public GetRecipeQuery(Guid id)
    {
        Id = id;
    }
}

public class GetRecipeQueryHandler : IRequestHandler<GetRecipeQuery, Result<RecipeResponse>>
{
    private readonly IRepository<Recipe> _recipeRepository;

    public GetRecipeQueryHandler(IRepository<Recipe> recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<Result<RecipeResponse>> Handle(GetRecipeQuery request, CancellationToken cancellationToken)
    {
        var recipe = await _recipeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (recipe == null)
            return Result.Fail(new ResourceNotFoundError("Recipe"));
        
        return  Result.Ok(Mapper.MapToRecipeResponse(recipe));
    }
}