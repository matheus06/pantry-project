using Microsoft.EntityFrameworkCore;

namespace Microservice.IdentityServer.Infra.Database;

public class IdentityServerContext : DbContext
{
    public IdentityServerContext(DbContextOptions<IdentityServerContext> options) : base(options)
    {
    }

   

    protected override void OnModelCreating(ModelBuilder builder)
    {

    }
}