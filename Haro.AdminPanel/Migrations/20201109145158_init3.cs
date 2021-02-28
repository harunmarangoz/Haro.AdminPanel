using Microsoft.EntityFrameworkCore.Migrations;

namespace Haro.AdminPanel.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TargetTableId",
                table: "Columns",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetTableId",
                table: "Columns");
        }
    }
}
