using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rr.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class TimeoutMs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeoutMs",
                table: "HttpMonitors",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeoutMs",
                table: "HttpMonitors");
        }
    }
}
