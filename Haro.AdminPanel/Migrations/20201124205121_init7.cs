using Microsoft.EntityFrameworkCore.Migrations;

namespace Haro.AdminPanel.Migrations
{
    public partial class init7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InputExtra",
                table: "Columns",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InputExtra",
                table: "Columns");
        }
    }
}
