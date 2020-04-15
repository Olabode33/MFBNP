using Microsoft.EntityFrameworkCore.Migrations;

namespace PMSDemo.Migrations
{
    public partial class Updated_InidicatorLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IndicatorUpdateLogs_PerformanceIndicators_PerformanceIndicatorId",
                table: "IndicatorUpdateLogs");

            migrationBuilder.DropIndex(
                name: "IX_IndicatorUpdateLogs_PerformanceIndicatorId",
                table: "IndicatorUpdateLogs");

            migrationBuilder.DropColumn(
                name: "ActualValue",
                table: "IndicatorUpdateLogs");

            migrationBuilder.DropColumn(
                name: "PerformanceIndicatorId",
                table: "IndicatorUpdateLogs");

            migrationBuilder.AddColumn<string>(
                name: "Actual",
                table: "IndicatorUpdateLogs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComparisonMethod",
                table: "IndicatorUpdateLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "IndicatorUpdateLogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "IndicatorUpdateLogs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IndicatorId",
                table: "IndicatorUpdateLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MeansOfVerification",
                table: "IndicatorUpdateLogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Target",
                table: "IndicatorUpdateLogs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetId",
                table: "IndicatorUpdateLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "IndicatorUpdateLogs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Actual",
                table: "IndicatorUpdateLogs");

            migrationBuilder.DropColumn(
                name: "ComparisonMethod",
                table: "IndicatorUpdateLogs");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "IndicatorUpdateLogs");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "IndicatorUpdateLogs");

            migrationBuilder.DropColumn(
                name: "IndicatorId",
                table: "IndicatorUpdateLogs");

            migrationBuilder.DropColumn(
                name: "MeansOfVerification",
                table: "IndicatorUpdateLogs");

            migrationBuilder.DropColumn(
                name: "Target",
                table: "IndicatorUpdateLogs");

            migrationBuilder.DropColumn(
                name: "TargetId",
                table: "IndicatorUpdateLogs");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "IndicatorUpdateLogs");

            migrationBuilder.AddColumn<string>(
                name: "ActualValue",
                table: "IndicatorUpdateLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PerformanceIndicatorId",
                table: "IndicatorUpdateLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_IndicatorUpdateLogs_PerformanceIndicatorId",
                table: "IndicatorUpdateLogs",
                column: "PerformanceIndicatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_IndicatorUpdateLogs_PerformanceIndicators_PerformanceIndicatorId",
                table: "IndicatorUpdateLogs",
                column: "PerformanceIndicatorId",
                principalTable: "PerformanceIndicators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
