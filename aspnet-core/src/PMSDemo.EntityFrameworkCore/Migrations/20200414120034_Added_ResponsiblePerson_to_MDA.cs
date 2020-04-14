using Microsoft.EntityFrameworkCore.Migrations;

namespace PMSDemo.Migrations
{
    public partial class Added_ResponsiblePerson_to_MDA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ResponsiblePersonId",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AbpOrganizationUnits",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponsiblePersonId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AbpOrganizationUnits");
        }
    }
}
