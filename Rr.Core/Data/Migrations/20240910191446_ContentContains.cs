using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rr.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class ContentContains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentContains",
                table: "HttpMonitors",
                type: "TEXT",
                maxLength: 5000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentContains",
                table: "HttpMonitors");
        }
    }
}
