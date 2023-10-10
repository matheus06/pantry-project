
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microservice.Scheduler.Infra.Database;

public class SchedulerContext : DbContext
{
    public SchedulerContext(DbContextOptions<SchedulerContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {

    }
}