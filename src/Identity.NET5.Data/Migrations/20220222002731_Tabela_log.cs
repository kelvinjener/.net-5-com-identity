using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.NET5.Data.Migrations
{
    public partial class Tabela_log : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_log",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipo = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    acao = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    data_ocorrencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    usuario = table.Column<string>(type: "nvarchar(max)", maxLength: 4256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("id", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_log");
        }
    }
}
