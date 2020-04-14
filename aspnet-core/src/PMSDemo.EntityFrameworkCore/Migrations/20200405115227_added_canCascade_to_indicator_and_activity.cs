using Microsoft.EntityFrameworkCore.Migrations;

namespace PMSDemo.Migrations
{
    public partial class added_canCascade_to_indicator_and_activity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanCascade",
                table: "PerformanceIndicators",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanCascade",
                table: "PerformanceActivitys",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanCascade",
                table: "PerformanceIndicators");

            migrationBuilder.DropColumn(
                name: "CanCascade",
                table: "PerformanceActivitys");
        }
    }
}
