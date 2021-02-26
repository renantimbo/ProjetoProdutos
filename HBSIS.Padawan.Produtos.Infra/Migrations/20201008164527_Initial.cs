using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HBSIS.Padawan.Produtos.Infra.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fornecedores",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    RazaoSocial = table.Column<string>(maxLength: 500, nullable: false),
                    Cnpj = table.Column<string>(maxLength: 14, nullable: false),
                    NomeFantasia = table.Column<string>(maxLength: 500, nullable: true),
                    Endereco = table.Column<string>(maxLength: 500, nullable: false),
                    Telefone = table.Column<string>(maxLength: 20, nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    Nome = table.Column<string>(maxLength: 500, nullable: false),
                    IdFornecedor = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categorias_Fornecedores_IdFornecedor",
                        column: x => x.IdFornecedor,
                        principalTable: "Fornecedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_IdFornecedor",
                table: "Categorias",
                column: "IdFornecedor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Fornecedores");
        }
    }
}
