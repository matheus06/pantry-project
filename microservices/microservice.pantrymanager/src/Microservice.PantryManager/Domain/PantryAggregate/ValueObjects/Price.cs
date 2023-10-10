using Platform.Domain.Abstractions;

namespace Microservice.PantryManager.Domain;

public class Price : ValueObject
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    public Price(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}