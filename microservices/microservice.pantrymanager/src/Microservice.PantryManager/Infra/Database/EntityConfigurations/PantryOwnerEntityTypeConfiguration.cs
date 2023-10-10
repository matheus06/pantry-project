using Microservice.PantryManager.Domain.PantryAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservice.PantryManager.Infra.Database.EntityConfigurations;

public class PantryOwnerEntityTypeConfiguration : IEntityTypeConfiguration<PantryOwner>
{
    public void Configure(EntityTypeBuilder<PantryOwner> builder)
    {
        builder.HasKey(p => p.Id);
    }
}