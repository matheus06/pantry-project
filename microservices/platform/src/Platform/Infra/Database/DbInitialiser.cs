using Microsoft.EntityFrameworkCore;

namespace Platform.Infra.Database;

public class DbInitializer
{
    private DbContext DbContext { get; }

    public DbInitializer(DbContext dbContext)
    {
        DbContext = dbContext;
    }

    public void Run()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Database.EnsureCreated();
    }
}