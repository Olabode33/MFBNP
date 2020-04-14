using Microsoft.EntityFrameworkCore.Migrations;

namespace PMSDemo.Migrations
{
    public partial class Addedattachmententities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityUpdateLogs_PerformanceActivitys_PerformanceActivityId",
                table: "ActivityUpdateLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PerformanceActivitys",
                table: "PerformanceActivitys");

            migrationBuilder.RenameTable(
                name: "PerformanceActivitys",
                newName: "PerformanceActivities");

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "IndicatorYearlyTargets",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataSource",
                table: "PerformanceActivities",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PerformanceActivities",
                table: "PerformanceActivities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityUpdateLogs_PerformanceActivities_PerformanceActivityId",
                table: "ActivityUpdateLogs",
                column: "PerformanceActivityId",
                principalTable: "PerformanceActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityUpdateLogs_PerformanceActivities_PerformanceActivityId",
                table: "ActivityUpdateLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PerformanceActivities",
                table: "PerformanceActivities");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "IndicatorYearlyTargets");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "PerformanceActivities");

            migrationBuilder.RenameTable(
                name: "PerformanceActivities",
                newName: "PerformanceActivitys");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PerformanceActivitys",
                table: "PerformanceActivitys",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityUpdateLogs_PerformanceActivitys_PerformanceActivityId",
                table: "ActivityUpdateLogs",
                column: "PerformanceActivityId",
                principalTable: "PerformanceActivitys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
