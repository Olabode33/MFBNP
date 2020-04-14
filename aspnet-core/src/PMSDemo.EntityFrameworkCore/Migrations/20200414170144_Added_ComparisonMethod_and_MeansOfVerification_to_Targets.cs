using Microsoft.EntityFrameworkCore.Migrations;

namespace PMSDemo.Migrations
{
    public partial class Added_ComparisonMethod_and_MeansOfVerification_to_Targets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ComparisonMethod",
                table: "IndicatorYearlyTargets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "IndicatorYearlyTargets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeansOfVerification",
                table: "IndicatorYearlyTargets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComparisonMethod",
                table: "IndicatorYearlyTargets");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "IndicatorYearlyTargets");

            migrationBuilder.DropColumn(
                name: "MeansOfVerification",
                table: "IndicatorYearlyTargets");
        }
    }
}
