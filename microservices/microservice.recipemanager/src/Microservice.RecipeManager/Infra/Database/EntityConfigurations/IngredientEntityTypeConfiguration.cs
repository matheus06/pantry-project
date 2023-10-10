using Microservice.RecipeManager.Domain;
using Microservice.RecipeManager.Domain.RecipeAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservice.RecipeManager.Infra.Database.EntityConfigurations;

public class IngredientEntityTypeConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.HasKey(i => i.Id);
        
        builder.Property(p => p.Name).HasMaxLength(100);
        builder.Property(p => p.Description).HasMaxLength(100);
    }
}

