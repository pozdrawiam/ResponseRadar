using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rr.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class ResponseTimeMs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResponseTimeMs",
                table: "HttpMonitors",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseTimeMs",
                table: "HttpMonitors");
        }
    }
}
