using Microservice.PantryManager.Domain;
using Microservice.PantryManager.Domain.PantryAggregate;
using Microservice.PantryManager.Domain.PantryAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservice.PantryManager.Infra.Database.EntityConfigurations;

public class PantryItemEntityTypeConfiguration : IEntityTypeConfiguration<PantryItem>
{
    public void Configure(EntityTypeBuilder<PantryItem> builder)
    {
        builder.HasKey(pantryItem => pantryItem.Id);
        builder.Property(pantryItem => pantryItem.Id).ValueGeneratedNever();
     
        builder.OwnsOne(pantryItem => pantryItem.ItemQuantity).Property(quantity=>quantity.Amount).HasColumnName("QuantityAmount");
        builder.OwnsOne(pantryItem => pantryItem.ItemQuantity).Property(quantity=>quantity.Unit).HasColumnName("QuantityUnit");
        
        builder.HasOne<Pantry>().WithMany(pantry=>pantry.Items).HasForeignKey(pantryItem=>pantryItem.PantryId).IsRequired();
        builder.HasOne<Product>().WithMany().HasForeignKey(pantryItem=>pantryItem.ProductId).IsRequired();
    }
}