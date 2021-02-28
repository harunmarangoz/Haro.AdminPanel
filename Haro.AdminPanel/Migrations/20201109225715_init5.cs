using Microsoft.EntityFrameworkCore.Migrations;

namespace Haro.AdminPanel.Migrations
{
    public partial class init5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ListOrder",
                table: "Columns",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListOrder",
                table: "Columns");
        }
    }
}
