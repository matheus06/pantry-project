using Microservice.RecipeManager.Domain.RecipeAggregate.Enums;
using Platform.Domain.Abstractions;

namespace Microservice.RecipeManager.Domain.RecipeAggregate.ValueObjects;

public class Quantity : ValueObject
{
    public decimal Amount { get; set; }
    public VolumeUnit Unit { get; set; }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Unit;

    }
}