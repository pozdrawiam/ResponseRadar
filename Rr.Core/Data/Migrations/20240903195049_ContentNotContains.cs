using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rr.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class ContentNotContains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ContentNotContains",
                table: "HttpMonitors",
                type: "TEXT",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 5000);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ContentNotContains",
                table: "HttpMonitors",
                type: "TEXT",
                maxLength: 5000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 5000,
                oldNullable: true);
        }
    }
}
