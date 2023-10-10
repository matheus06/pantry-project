using Microservice.RecipeManager.Domain;
using Microservice.RecipeManager.Domain.RecipeAggregate;
using Microservice.RecipeManager.Domain.RecipeAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservice.RecipeManager.Infra.Database.EntityConfigurations;

public class RecipeIngredientEntityTypeConfiguration : IEntityTypeConfiguration<RecipeIngredient>
{
    public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
    {
        builder.HasKey(ri => ri.Id);
        
        builder.OwnsOne(ri => ri.IngredientQuantity).Property(q=> q.Amount).HasColumnName("IngredientAmount");
        builder.OwnsOne(ri => ri.IngredientQuantity).Property(q=> q.Unit).HasColumnName("IngredientUnit");

        builder.HasOne<Recipe>().WithMany(r=>r.Ingredients).HasForeignKey(ri=>ri.RecipeId).IsRequired();
        builder.HasOne<Ingredient>().WithMany().HasForeignKey(ri=>ri.IngredientId).IsRequired();
    }
}

