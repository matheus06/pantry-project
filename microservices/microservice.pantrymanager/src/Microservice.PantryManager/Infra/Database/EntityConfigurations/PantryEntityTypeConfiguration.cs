using Microservice.PantryManager.Domain.PantryAggregate;
using Microservice.PantryManager.Domain.PantryAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservice.PantryManager.Infra.Database.EntityConfigurations;

public class PantryEntityTypeConfiguration : IEntityTypeConfiguration<Pantry>
{
   
    public void Configure(EntityTypeBuilder<Pantry> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Name).HasMaxLength(100);
        builder.Property(p => p.Description).HasMaxLength(100);
        
        builder.HasOne<PantryOwner>().WithMany().HasForeignKey(pantryItem=>pantryItem.PantryOwnerId).IsRequired();
        
        builder.Navigation(p=>p.Items).Metadata.SetField("_pantryItems");
        builder.Navigation(p=>p.Items).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

