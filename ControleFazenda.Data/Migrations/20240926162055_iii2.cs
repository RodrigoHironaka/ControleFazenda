using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleFazenda.Data.Migrations
{
    /// <inheritdoc />
    public partial class iii2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CaminhoArquivo",
                table: "NFes",
                type: "varchar(500)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaminhoArquivo",
                table: "NFes");
        }
    }
}
