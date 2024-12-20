﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleFazenda.Data.Migrations
{
    /// <inheritdoc />
    public partial class ini : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(100)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(100)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(100)", nullable: false),
                    Fazenda = table.Column<int>(type: "int", nullable: false),
                    AcessoTotal = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "varchar(100)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(100)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(100)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(100)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "varchar(100)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(100)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(100)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Caixas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Situacao = table.Column<int>(type: "int", nullable: false),
                    Numero = table.Column<long>(type: "bigint", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCadastroId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioAlteracaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fazenda = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caixas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Colaboradores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Admissao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Demissao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Situacao = table.Column<int>(type: "int", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCadastroId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioAlteracaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fazenda = table.Column<int>(type: "int", nullable: false),
                    RazaoSocial = table.Column<string>(type: "varchar(200)", nullable: false),
                    NomeFantasia = table.Column<string>(type: "varchar(200)", nullable: true),
                    Nascimento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Documento = table.Column<long>(type: "bigint", nullable: false),
                    Documento2 = table.Column<string>(type: "varchar(20)", nullable: true),
                    Telefone = table.Column<string>(type: "varchar(20)", nullable: true),
                    Celular = table.Column<string>(type: "varchar(20)", nullable: true),
                    Celular2 = table.Column<string>(type: "varchar(20)", nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", nullable: true),
                    TipoPessoa = table.Column<int>(type: "int", nullable: false),
                    Logradouro = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Numero = table.Column<string>(type: "varchar(100)", maxLength: 30, nullable: true),
                    Bairro = table.Column<string>(type: "varchar(100)", maxLength: 70, nullable: true),
                    Complemento = table.Column<string>(type: "varchar(100)", maxLength: 150, nullable: true),
                    Referencia = table.Column<string>(type: "varchar(100)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colaboradores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormasPagamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    QtdParcelamento = table.Column<int>(type: "int", nullable: false),
                    PeriodoParcelamento = table.Column<int>(type: "int", nullable: false),
                    Situacao = table.Column<int>(type: "int", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCadastroId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioAlteracaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fazenda = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormasPagamento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fornecedores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Situacao = table.Column<int>(type: "int", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCadastroId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioAlteracaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fazenda = table.Column<int>(type: "int", nullable: false),
                    RazaoSocial = table.Column<string>(type: "varchar(200)", nullable: false),
                    NomeFantasia = table.Column<string>(type: "varchar(200)", nullable: true),
                    Nascimento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Documento = table.Column<long>(type: "bigint", nullable: false),
                    Documento2 = table.Column<string>(type: "varchar(20)", nullable: true),
                    Telefone = table.Column<string>(type: "varchar(20)", nullable: true),
                    Celular = table.Column<string>(type: "varchar(20)", nullable: true),
                    Celular2 = table.Column<string>(type: "varchar(20)", nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", nullable: true),
                    TipoPessoa = table.Column<int>(type: "int", nullable: false),
                    Logradouro = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Numero = table.Column<string>(type: "varchar(100)", maxLength: 30, nullable: true),
                    Bairro = table.Column<string>(type: "varchar(100)", maxLength: 70, nullable: true),
                    Complemento = table.Column<string>(type: "varchar(100)", maxLength: 150, nullable: true),
                    Referencia = table.Column<string>(type: "varchar(100)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogsAlteracao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Chave = table.Column<string>(type: "varchar(200)", nullable: false),
                    Historico = table.Column<string>(type: "varchar(8000)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCadastroId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioAlteracaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fazenda = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsAlteracao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Malote",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    QuemLevou = table.Column<string>(type: "varchar(200)", nullable: true),
                    Enviado = table.Column<bool>(type: "bit", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(8000)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCadastroId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioAlteracaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fazenda = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Malote", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "varchar(100)", nullable: false),
                    ClaimType = table.Column<string>(type: "varchar(100)", nullable: true),
                    ClaimValue = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "varchar(100)", nullable: false),
                    ClaimType = table.Column<string>(type: "varchar(100)", nullable: true),
                    ClaimValue = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(100)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(100)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "varchar(100)", nullable: true),
                    UserId = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(100)", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(100)", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(100)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Diaristas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ColaboradorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(8000)", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCadastroId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioAlteracaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fazenda = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diaristas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diaristas_Colaboradores_ColaboradorId",
                        column: x => x.ColaboradorId,
                        principalTable: "Colaboradores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Recibos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Numero = table.Column<string>(type: "varchar(50)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(10,5)", precision: 10, scale: 5, nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Referente = table.Column<string>(type: "varchar(500)", nullable: true),
                    NumeroCheque = table.Column<string>(type: "varchar(100)", nullable: true),
                    BancoCheque = table.Column<string>(type: "varchar(50)", nullable: true),
                    ContaCheque = table.Column<string>(type: "varchar(50)", nullable: true),
                    ColaboradorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCadastroId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioAlteracaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fazenda = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recibos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recibos_Colaboradores_ColaboradorId",
                        column: x => x.ColaboradorId,
                        principalTable: "Colaboradores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Vales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Numero = table.Column<long>(type: "bigint", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(10,5)", precision: 10, scale: 5, nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AutorizadoPor = table.Column<string>(type: "varchar(100)", nullable: false),
                    Situacao = table.Column<int>(type: "int", nullable: true),
                    ColaboradorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCadastroId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioAlteracaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fazenda = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vales_Colaboradores_ColaboradorId",
                        column: x => x.ColaboradorId,
                        principalTable: "Colaboradores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FluxosCaixa",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(200)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(10,5)", precision: 10, scale: 5, nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DebitoCredito = table.Column<int>(type: "int", nullable: false),
                    FormaPagamentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaixaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCadastroId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioAlteracaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fazenda = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FluxosCaixa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FluxosCaixa_Caixas_CaixaId",
                        column: x => x.CaixaId,
                        principalTable: "Caixas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FluxosCaixa_FormasPagamento_FormaPagamentoId",
                        column: x => x.FormaPagamentoId,
                        principalTable: "FormasPagamento",
                        principalColumn: "Id");
                });

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
                    CaminhoArquivo = table.Column<string>(type: "varchar(500)", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCadastroId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioAlteracaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fazenda = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Diarias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoPeriodo = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntradaManha = table.Column<TimeSpan>(type: "time", nullable: true),
                    SaidaManha = table.Column<TimeSpan>(type: "time", nullable: true),
                    EntradaTarde = table.Column<TimeSpan>(type: "time", nullable: true),
                    SaidaTarde = table.Column<TimeSpan>(type: "time", nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(10,5)", precision: 10, scale: 5, nullable: true),
                    Identificador = table.Column<string>(type: "varchar(100)", nullable: true),
                    SituacaoPagamento = table.Column<int>(type: "int", nullable: false),
                    DiaristaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCadastroId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioAlteracaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fazenda = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diarias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diarias_Diaristas_DiaristaId",
                        column: x => x.DiaristaId,
                        principalTable: "Diaristas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Diarias_DiaristaId",
                table: "Diarias",
                column: "DiaristaId");

            migrationBuilder.CreateIndex(
                name: "IX_Diaristas_ColaboradorId",
                table: "Diaristas",
                column: "ColaboradorId");

            migrationBuilder.CreateIndex(
                name: "IX_FluxosCaixa_CaixaId",
                table: "FluxosCaixa",
                column: "CaixaId");

            migrationBuilder.CreateIndex(
                name: "IX_FluxosCaixa_FormaPagamentoId",
                table: "FluxosCaixa",
                column: "FormaPagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_NFes_FornecedorId",
                table: "NFes",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Recibos_ColaboradorId",
                table: "Recibos",
                column: "ColaboradorId");

            migrationBuilder.CreateIndex(
                name: "IX_Vales_ColaboradorId",
                table: "Vales",
                column: "ColaboradorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Diarias");

            migrationBuilder.DropTable(
                name: "FluxosCaixa");

            migrationBuilder.DropTable(
                name: "LogsAlteracao");

            migrationBuilder.DropTable(
                name: "Malote");

            migrationBuilder.DropTable(
                name: "NFes");

            migrationBuilder.DropTable(
                name: "Recibos");

            migrationBuilder.DropTable(
                name: "Vales");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Diaristas");

            migrationBuilder.DropTable(
                name: "Caixas");

            migrationBuilder.DropTable(
                name: "FormasPagamento");

            migrationBuilder.DropTable(
                name: "Fornecedores");

            migrationBuilder.DropTable(
                name: "Colaboradores");
        }
    }
}
