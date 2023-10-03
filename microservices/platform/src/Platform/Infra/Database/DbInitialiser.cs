using Microsoft.EntityFrameworkCore;

namespace Platform.Infra.Database;

public class DbInitializer
{
    private DbContext DbContext { get; }
    public bool IsToUseMigration { get; set; }

    public DbInitializer(DbContext dbContext)
    {
        DbContext = dbContext;
    }

    public void Run()
    {
        if(IsToUseMigration)
        {
            DbContext.Database.Migrate();
        }
        else
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Database.EnsureCreated();
        }   
    }
}