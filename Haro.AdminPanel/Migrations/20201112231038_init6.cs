using Microsoft.EntityFrameworkCore.Migrations;

namespace Haro.AdminPanel.Migrations
{
    public partial class init6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WebProjectName",
                table: "SiteInformations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WebProjectName",
                table: "SiteInformations");
        }
    }
}
