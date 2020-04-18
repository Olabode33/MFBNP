using Microsoft.EntityFrameworkCore.Migrations;

namespace PMSDemo.Migrations
{
    public partial class Added_Percentage_Achieved_to_indicators : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PercentageAchieved",
                table: "PerformanceIndicators",
                nullable: false,
                defaultValue: 70);

            migrationBuilder.AddColumn<int>(
                name: "PercentageAchieved",
                table: "IndicatorYearlyTargets",
                nullable: false,
                defaultValue: 70);

            migrationBuilder.AddColumn<int>(
                name: "PercentageAchieved",
                table: "IndicatorUpdateLogs",
                nullable: false,
                defaultValue: 70);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentageAchieved",
                table: "PerformanceIndicators");

            migrationBuilder.DropColumn(
                name: "PercentageAchieved",
                table: "IndicatorYearlyTargets");

            migrationBuilder.DropColumn(
                name: "PercentageAchieved",
                table: "IndicatorUpdateLogs");
        }
    }
}
