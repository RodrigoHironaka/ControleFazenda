using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleFazenda.Data.Migrations
{
    /// <inheritdoc />
    public partial class ini2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Data",
                table: "Malote",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "Malote");
        }
    }
}
