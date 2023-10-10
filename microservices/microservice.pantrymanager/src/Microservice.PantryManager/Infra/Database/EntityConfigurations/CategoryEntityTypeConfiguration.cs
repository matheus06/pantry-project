using Microservice.PantryManager.Domain.PantryAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservice.PantryManager.Infra.Database.EntityConfigurations;

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> categoryConfiguration)
    {
        categoryConfiguration.HasKey(e => e.Id);
    }
}
