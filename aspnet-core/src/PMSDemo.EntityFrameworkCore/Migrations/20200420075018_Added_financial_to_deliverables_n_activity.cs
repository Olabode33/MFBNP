using Microsoft.EntityFrameworkCore.Migrations;

namespace PMSDemo.Migrations
{
    public partial class Added_financial_to_deliverables_n_activity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AmountSpent",
                table: "PerformanceActivities",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BudgetAmount",
                table: "PerformanceActivities",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AmountSpent",
                table: "ActivityUpdateLogs",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BudgetAmount",
                table: "ActivityUpdateLogs",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AmountSpent",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BudgetAmount",
                table: "AbpOrganizationUnits",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountSpent",
                table: "PerformanceActivities");

            migrationBuilder.DropColumn(
                name: "BudgetAmount",
                table: "PerformanceActivities");

            migrationBuilder.DropColumn(
                name: "AmountSpent",
                table: "ActivityUpdateLogs");

            migrationBuilder.DropColumn(
                name: "BudgetAmount",
                table: "ActivityUpdateLogs");

            migrationBuilder.DropColumn(
                name: "AmountSpent",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "BudgetAmount",
                table: "AbpOrganizationUnits");
        }
    }
}
