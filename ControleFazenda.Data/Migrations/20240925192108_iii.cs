using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleFazenda.Data.Migrations
{
    /// <inheritdoc />
    public partial class iii : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NFes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Numero = table.Column<long>(type: "bigint", nullable: true),
                    FornecedorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(10,5)", precision: 10, scale: 5, nullable: true),
                    Emissao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecebimentoNFe = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TipoNFe = table.Column<int>(type: "int", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCadastroId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioAlteracaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NFes_Fornecedores_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NFes_FornecedorId",
                table: "NFes",
                column: "FornecedorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NFes");
        }
    }
}
