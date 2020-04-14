using Microsoft.EntityFrameworkCore.Migrations;

namespace PMSDemo.Migrations
{
    public partial class Updatedperformancereview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Challenges",
                table: "PerformanceReviews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Recommendation",
                table: "PerformanceReviews",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Challenges",
                table: "PerformanceReviews");

            migrationBuilder.DropColumn(
                name: "Recommendation",
                table: "PerformanceReviews");
        }
    }
}
