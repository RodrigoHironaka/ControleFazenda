using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleFazenda.Data.Migrations
{
    /// <inheritdoc />
    public partial class ini3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diarias_Diaristas_DiaristaId",
                table: "Diarias");

            migrationBuilder.DropForeignKey(
                name: "FK_Diaristas_Colaboradores_ColaboradorId",
                table: "Diaristas");

            migrationBuilder.DropForeignKey(
                name: "FK_FluxosCaixa_Caixas_CaixaId",
                table: "FluxosCaixa");

            migrationBuilder.DropForeignKey(
                name: "FK_FluxosCaixa_FormasPagamento_FormaPagamentoId",
                table: "FluxosCaixa");

            migrationBuilder.DropForeignKey(
                name: "FK_Recibos_Colaboradores_ColaboradorId",
                table: "Recibos");

            migrationBuilder.DropForeignKey(
                name: "FK_Vales_Colaboradores_ColaboradorId",
                table: "Vales");

            migrationBuilder.AddForeignKey(
                name: "FK_Diarias_Diaristas_DiaristaId",
                table: "Diarias",
                column: "DiaristaId",
                principalTable: "Diaristas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Diaristas_Colaboradores_ColaboradorId",
                table: "Diaristas",
                column: "ColaboradorId",
                principalTable: "Colaboradores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FluxosCaixa_Caixas_CaixaId",
                table: "FluxosCaixa",
                column: "CaixaId",
                principalTable: "Caixas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FluxosCaixa_FormasPagamento_FormaPagamentoId",
                table: "FluxosCaixa",
                column: "FormaPagamentoId",
                principalTable: "FormasPagamento",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recibos_Colaboradores_ColaboradorId",
                table: "Recibos",
                column: "ColaboradorId",
                principalTable: "Colaboradores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vales_Colaboradores_ColaboradorId",
                table: "Vales",
                column: "ColaboradorId",
                principalTable: "Colaboradores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diarias_Diaristas_DiaristaId",
                table: "Diarias");

            migrationBuilder.DropForeignKey(
                name: "FK_Diaristas_Colaboradores_ColaboradorId",
                table: "Diaristas");

            migrationBuilder.DropForeignKey(
                name: "FK_FluxosCaixa_Caixas_CaixaId",
                table: "FluxosCaixa");

            migrationBuilder.DropForeignKey(
                name: "FK_FluxosCaixa_FormasPagamento_FormaPagamentoId",
                table: "FluxosCaixa");

            migrationBuilder.DropForeignKey(
                name: "FK_Recibos_Colaboradores_ColaboradorId",
                table: "Recibos");

            migrationBuilder.DropForeignKey(
                name: "FK_Vales_Colaboradores_ColaboradorId",
                table: "Vales");

            migrationBuilder.AddForeignKey(
                name: "FK_Diarias_Diaristas_DiaristaId",
                table: "Diarias",
                column: "DiaristaId",
                principalTable: "Diaristas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Diaristas_Colaboradores_ColaboradorId",
                table: "Diaristas",
                column: "ColaboradorId",
                principalTable: "Colaboradores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FluxosCaixa_Caixas_CaixaId",
                table: "FluxosCaixa",
                column: "CaixaId",
                principalTable: "Caixas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FluxosCaixa_FormasPagamento_FormaPagamentoId",
                table: "FluxosCaixa",
                column: "FormaPagamentoId",
                principalTable: "FormasPagamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recibos_Colaboradores_ColaboradorId",
                table: "Recibos",
                column: "ColaboradorId",
                principalTable: "Colaboradores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vales_Colaboradores_ColaboradorId",
                table: "Vales",
                column: "ColaboradorId",
                principalTable: "Colaboradores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
