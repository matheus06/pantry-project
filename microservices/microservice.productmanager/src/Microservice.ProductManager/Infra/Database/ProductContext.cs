using Microservice.ProductManager.Domain;
using Microservice.ProductManager.Infra.Database.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Microservice.ProductManager.Infra.Database;

public class ProductContext : DbContext
{
    public ProductContext(DbContextOptions<ProductContext> options) : base(options)
    {
    }
    
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder.ApplyConfiguration(new ProductEntityTypeConfiguration());
    }
}