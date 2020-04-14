using Microsoft.EntityFrameworkCore.Migrations;

namespace PMSDemo.Migrations
{
    public partial class Updated_Deliverable_to_include_PriorityArea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpOrganizationUnits_AbpOrganizationUnits_MdaId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropIndex(
                name: "IX_AbpOrganizationUnits_MdaId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "MdaId",
                table: "AbpOrganizationUnits");

            migrationBuilder.AddColumn<int>(
                name: "PriorityAreaId",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_PriorityAreaId",
                table: "AbpOrganizationUnits",
                column: "PriorityAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpOrganizationUnits_PriorityAreas_PriorityAreaId",
                table: "AbpOrganizationUnits",
                column: "PriorityAreaId",
                principalTable: "PriorityAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpOrganizationUnits_PriorityAreas_PriorityAreaId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropIndex(
                name: "IX_AbpOrganizationUnits_PriorityAreaId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "PriorityAreaId",
                table: "AbpOrganizationUnits");

            migrationBuilder.AddColumn<long>(
                name: "MdaId",
                table: "AbpOrganizationUnits",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_MdaId",
                table: "AbpOrganizationUnits",
                column: "MdaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpOrganizationUnits_AbpOrganizationUnits_MdaId",
                table: "AbpOrganizationUnits",
                column: "MdaId",
                principalTable: "AbpOrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
