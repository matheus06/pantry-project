namespace Microservice.PantryManager.Application.Dto;

public class AddPantryItemRequest
{
    public Guid ProductId { get; set; }
    public QuantityDto Quantity { get; set; }
}