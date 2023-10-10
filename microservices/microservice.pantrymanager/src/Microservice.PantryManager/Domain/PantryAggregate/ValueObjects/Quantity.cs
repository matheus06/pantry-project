using Microservice.PantryManager.Domain.PantryAggregate.Enums;
using Platform.Domain.Abstractions;

namespace Microservice.PantryManager.Domain.PantryAggregate.ValueObjects;

public class Quantity : ValueObject
{
    public decimal Amount { get; internal set; }
    public VolumeUnit Unit { get; internal set; }
    
    public Quantity(decimal amount, VolumeUnit unit)
    {
        Amount = amount;
        Unit = unit;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Unit;

    }
}