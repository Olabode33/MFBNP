using Microsoft.EntityFrameworkCore.Migrations;

namespace PMSDemo.Migrations
{
    public partial class Updatedtargetattachments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IndicatorAttachments_PerformanceIndicators_PerformanceIndicatorId",
                table: "IndicatorAttachments");

            migrationBuilder.DropIndex(
                name: "IX_IndicatorAttachments_PerformanceIndicatorId",
                table: "IndicatorAttachments");

            migrationBuilder.DropColumn(
                name: "PerformanceIndicatorId",
                table: "IndicatorAttachments");

            migrationBuilder.AddColumn<int>(
                name: "IndicatorTargetId",
                table: "IndicatorAttachments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_IndicatorAttachments_IndicatorTargetId",
                table: "IndicatorAttachments",
                column: "IndicatorTargetId");

            migrationBuilder.AddForeignKey(
                name: "FK_IndicatorAttachments_IndicatorYearlyTargets_IndicatorTargetId",
                table: "IndicatorAttachments",
                column: "IndicatorTargetId",
                principalTable: "IndicatorYearlyTargets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IndicatorAttachments_IndicatorYearlyTargets_IndicatorTargetId",
                table: "IndicatorAttachments");

            migrationBuilder.DropIndex(
                name: "IX_IndicatorAttachments_IndicatorTargetId",
                table: "IndicatorAttachments");

            migrationBuilder.DropColumn(
                name: "IndicatorTargetId",
                table: "IndicatorAttachments");

            migrationBuilder.AddColumn<int>(
                name: "PerformanceIndicatorId",
                table: "IndicatorAttachments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_IndicatorAttachments_PerformanceIndicatorId",
                table: "IndicatorAttachments",
                column: "PerformanceIndicatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_IndicatorAttachments_PerformanceIndicators_PerformanceIndicatorId",
                table: "IndicatorAttachments",
                column: "PerformanceIndicatorId",
                principalTable: "PerformanceIndicators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
