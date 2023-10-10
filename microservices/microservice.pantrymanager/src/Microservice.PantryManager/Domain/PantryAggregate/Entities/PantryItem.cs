using Microservice.PantryManager.Domain.PantryAggregate.ValueObjects;
using Platform.Domain.Abstractions;

namespace Microservice.PantryManager.Domain.PantryAggregate.Entities
{
    public class PantryItem : Entity<Guid>
    {
        public Guid PantryId { get; private set; }
        public Guid ProductId { get; private set; }
        public Quantity ItemQuantity { get; private set; }
        
        protected PantryItem( ){ }

        public PantryItem(Guid pantryId, Guid productId, Quantity itemQuantity) 
            : base(Guid.NewGuid())
        {
            PantryId = pantryId;
            ProductId = productId;
            ItemQuantity = itemQuantity;
        }

        public void IncrementQuantity(Quantity quantity)
        {
            ItemQuantity.Amount += quantity.Amount;
        }
    }
}
