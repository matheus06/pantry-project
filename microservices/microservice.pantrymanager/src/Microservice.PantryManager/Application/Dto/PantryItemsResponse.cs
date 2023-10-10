namespace Microservice.PantryManager.Application.Dto;

public class PantryItemsResponse
{
    public Guid ProductId { get;  set; }
    public QuantityDto Quantity { get;  set; }
}