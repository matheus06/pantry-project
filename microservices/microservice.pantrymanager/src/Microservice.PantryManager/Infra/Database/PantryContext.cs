using Microservice.PantryManager.Domain;
using Microservice.PantryManager.Domain.PantryAggregate;
using Microservice.PantryManager.Domain.PantryAggregate.Entities;
using Microservice.PantryManager.Infra.Database.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Microservice.PantryManager.Infra.Database;

public class PantryContext : DbContext
{
    public PantryContext(DbContextOptions<PantryContext> options) : base(options)
    {
    }

    public DbSet<PantryItem> PantryItems { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Pantry> Pantries { get; set; }
    public DbSet<PantryOwner> PantryOwners { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
        builder.ApplyConfiguration(new PantryEntityTypeConfiguration());
        builder.ApplyConfiguration(new PantryOwnerEntityTypeConfiguration());
        builder.ApplyConfiguration(new PantryItemEntityTypeConfiguration());
    }
}