using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Sqlite.Internal;

#nullable disable

namespace Microservice.Scheduler.Infra.Database.Migrations
{
    /// <inheritdoc />
    public partial class RunHangfireScript : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sqlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Infra/Database/Migrations/hangfire-schema.sql");
            migrationBuilder.Sql(File.ReadAllText(sqlFile));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
