using Microservice.PantryManager.Domain;
using Microservice.PantryManager.Domain.PantryAggregate.Entities;

namespace Microservice.PantryManager.Application.Dto;

public class PantryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IEnumerable<PantryItemsResponse> Items { get; set; }
}