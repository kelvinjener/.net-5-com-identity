using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.NET5.Data.Migrations
{
    public partial class rename_column_id_log : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tb_log",
                newName: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "tb_log",
                newName: "Id");
        }
    }
}
