using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rr.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class CheckUrlInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CheckedAt",
                table: "HttpMonitors",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "HttpMonitors",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckedAt",
                table: "HttpMonitors");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "HttpMonitors");
        }
    }
}
