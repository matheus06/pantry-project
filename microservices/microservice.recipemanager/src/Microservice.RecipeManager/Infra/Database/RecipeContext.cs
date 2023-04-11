using Microservice.RecipeManager.Domain;
using Microservice.RecipeManager.Domain.RecipeAggregate;
using Microservice.RecipeManager.Domain.RecipeAggregate.Entities;
using Microservice.RecipeManager.Infra.Database.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Microservice.RecipeManager.Infra.Database;

public class RecipeContext : DbContext
{
    public RecipeContext(DbContextOptions<RecipeContext> options) : base(options)
    {
    }
    
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new RecipeEntityTypeConfiguration());
        builder.ApplyConfiguration(new IngredientEntityTypeConfiguration());
        builder.ApplyConfiguration(new RecipeIngredientEntityTypeConfiguration());
    }
}