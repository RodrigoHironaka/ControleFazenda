﻿// <auto-generated />
using System;
using ControleFazenda.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ControleFazenda.Data.Migrations
{
    [DbContext(typeof(ContextoPrincipal))]
    [Migration("20240925192108_iii")]
    partial class iii
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ControleFazenda.Business.Entidades.Caixa", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataAlteracao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<long>("Numero")
                        .HasColumnType("bigint");

                    b.Property<int>("Situacao")
                        .HasColumnType("int");

                    b.Property<Guid>("UsuarioAlteracaoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsuarioCadastroId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Caixas", (string)null);
                });

            modelBuilder.Entity("ControleFazenda.Business.Entidades.Colaborador", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("Admissao")
                        .HasColumnType("datetime2");

                    b.Property<string>("Celular")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Celular2")
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("DataAlteracao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Demissao")
                        .HasColumnType("datetime2");

                    b.Property<long>("Documento")
                        .HasColumnType("bigint");

                    b.Property<string>("Documento2")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime?>("Nascimento")
                        .HasColumnType("datetime2");

                    b.Property<string>("NomeFantasia")
                        .HasColumnType("varchar(200)");

                    b.Property<string>("RazaoSocial")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<int>("Situacao")
                        .HasColumnType("int");

                    b.Property<string>("Telefone")
                        .HasColumnType("varchar(20)");

                    b.Property<int>("TipoPessoa")
                        .HasColumnType("int");

                    b.Property<Guid>("UsuarioAlteracaoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsuarioCadastroId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Colaboradores", (string)null);
                });

            modelBuilder.Entity("ControleFazenda.Business.Entidades.FluxoCaixa", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CaixaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataAlteracao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<int>("DebitoCredito")
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<Guid>("FormaPagamentoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsuarioAlteracaoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsuarioCadastroId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Valor")
                        .HasPrecision(10, 5)
                        .HasColumnType("decimal(10,5)");

                    b.HasKey("Id");

                    b.HasIndex("CaixaId");

                    b.HasIndex("FormaPagamentoId");

                    b.ToTable("FluxosCaixa", (string)null);
                });

            modelBuilder.Entity("ControleFazenda.Business.Entidades.FormaPagamento", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataAlteracao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<int>("PeriodoParcelamento")
                        .HasColumnType("int");

                    b.Property<int>("QtdParcelamento")
                        .HasColumnType("int");

                    b.Property<int>("Situacao")
                        .HasColumnType("int");

                    b.Property<Guid>("UsuarioAlteracaoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsuarioCadastroId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("FormasPagamento", (string)null);
                });

            modelBuilder.Entity("ControleFazenda.Business.Entidades.Fornecedor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Celular")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Celular2")
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("DataAlteracao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<long>("Documento")
                        .HasColumnType("bigint");

                    b.Property<string>("Documento2")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime?>("Nascimento")
                        .HasColumnType("datetime2");

                    b.Property<string>("NomeFantasia")
                        .HasColumnType("varchar(200)");

                    b.Property<string>("RazaoSocial")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<int>("Situacao")
                        .HasColumnType("int");

                    b.Property<string>("Telefone")
                        .HasColumnType("varchar(20)");

                    b.Property<int>("TipoPessoa")
                        .HasColumnType("int");

                    b.Property<Guid>("UsuarioAlteracaoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsuarioCadastroId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Fornecedores", (string)null);
                });

            modelBuilder.Entity("ControleFazenda.Business.Entidades.LogAlteracao", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Chave")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime>("DataAlteracao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Historico")
                        .IsRequired()
                        .HasColumnType("varchar(8000)");

                    b.Property<Guid>("UsuarioAlteracaoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsuarioCadastroId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("LogsAlteracao", (string)null);
                });

            modelBuilder.Entity("ControleFazenda.Business.Entidades.NFe", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataAlteracao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Emissao")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("FornecedorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long?>("Numero")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("RecebimentoNFe")
                        .HasColumnType("datetime2");

                    b.Property<int>("TipoNFe")
                        .HasColumnType("int");

                    b.Property<Guid>("UsuarioAlteracaoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsuarioCadastroId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal?>("Valor")
                        .HasPrecision(10, 5)
                        .HasColumnType("decimal(10,5)");

                    b.HasKey("Id");

                    b.HasIndex("FornecedorId");

                    b.ToTable("NFes", (string)null);
                });

            modelBuilder.Entity("ControleFazenda.Business.Entidades.Recibo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BancoCheque")
                        .HasColumnType("varchar(50)");

                    b.Property<Guid>("ColaboradorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContaCheque")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataAlteracao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("NumeroCheque")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Referente")
                        .HasColumnType("varchar(500)");

                    b.Property<Guid>("UsuarioAlteracaoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsuarioCadastroId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Valor")
                        .HasPrecision(10, 5)
                        .HasColumnType("decimal(10,5)");

                    b.HasKey("Id");

                    b.HasIndex("ColaboradorId");

                    b.ToTable("Recibos", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(100)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(100)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Value")
                        .HasColumnType("varchar(100)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ControleFazenda.Business.Entidades.Colaborador", b =>
                {
                    b.OwnsOne("ControleFazenda.Business.Entidades.Componentes.Endereco", "Endereco", b1 =>
                        {
                            b1.Property<Guid>("ColaboradorId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Bairro")
                                .HasMaxLength(70)
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Bairro");

                            b1.Property<string>("Complemento")
                                .HasMaxLength(150)
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Complemento");

                            b1.Property<string>("Logradouro")
                                .HasMaxLength(100)
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Logradouro");

                            b1.Property<string>("Numero")
                                .HasMaxLength(30)
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Numero");

                            b1.Property<string>("Referencia")
                                .HasMaxLength(150)
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Referencia");

                            b1.HasKey("ColaboradorId");

                            b1.ToTable("Colaboradores");

                            b1.WithOwner()
                                .HasForeignKey("ColaboradorId");
                        });

                    b.Navigation("Endereco");
                });

            modelBuilder.Entity("ControleFazenda.Business.Entidades.FluxoCaixa", b =>
                {
                    b.HasOne("ControleFazenda.Business.Entidades.Caixa", "Caixa")
                        .WithMany("FluxosCaixa")
                        .HasForeignKey("CaixaId")
                        .IsRequired();

                    b.HasOne("ControleFazenda.Business.Entidades.FormaPagamento", "FormaPagamento")
                        .WithMany("FluxosCaixa")
                        .HasForeignKey("FormaPagamentoId")
                        .IsRequired();

                    b.Navigation("Caixa");

                    b.Navigation("FormaPagamento");
                });

            modelBuilder.Entity("ControleFazenda.Business.Entidades.Fornecedor", b =>
                {
                    b.OwnsOne("ControleFazenda.Business.Entidades.Componentes.Endereco", "Endereco", b1 =>
                        {
                            b1.Property<Guid>("FornecedorId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Bairro")
                                .HasMaxLength(70)
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Bairro");

                            b1.Property<string>("Complemento")
                                .HasMaxLength(150)
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Complemento");

                            b1.Property<string>("Logradouro")
                                .HasMaxLength(100)
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Logradouro");

                            b1.Property<string>("Numero")
                                .HasMaxLength(30)
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Numero");

                            b1.Property<string>("Referencia")
                                .HasMaxLength(150)
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Referencia");

                            b1.HasKey("FornecedorId");

                            b1.ToTable("Fornecedores");

                            b1.WithOwner()
                                .HasForeignKey("FornecedorId");
                        });

                    b.Navigation("Endereco");
                });

            modelBuilder.Entity("ControleFazenda.Business.Entidades.NFe", b =>
                {
                    b.HasOne("ControleFazenda.Business.Entidades.Fornecedor", "Fornecedor")
                        .WithMany("NFes")
                        .HasForeignKey("FornecedorId");

                    b.Navigation("Fornecedor");
                });

            modelBuilder.Entity("ControleFazenda.Business.Entidades.Recibo", b =>
                {
                    b.HasOne("ControleFazenda.Business.Entidades.Colaborador", "Colaborador")
                        .WithMany("Recibos")
                        .HasForeignKey("ColaboradorId")
                        .IsRequired();

                    b.Navigation("Colaborador");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ControleFazenda.Business.Entidades.Caixa", b =>
                {
                    b.Navigation("FluxosCaixa");
                });

            modelBuilder.Entity("ControleFazenda.Business.Entidades.Colaborador", b =>
                {
                    b.Navigation("Recibos");
                });

            modelBuilder.Entity("ControleFazenda.Business.Entidades.FormaPagamento", b =>
                {
                    b.Navigation("FluxosCaixa");
                });

            modelBuilder.Entity("ControleFazenda.Business.Entidades.Fornecedor", b =>
                {
                    b.Navigation("NFes");
                });
#pragma warning restore 612, 618
        }
    }
}
