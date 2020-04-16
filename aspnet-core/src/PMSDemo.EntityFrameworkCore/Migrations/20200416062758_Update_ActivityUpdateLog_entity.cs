using Microsoft.EntityFrameworkCore.Migrations;

namespace PMSDemo.Migrations
{
    public partial class Update_ActivityUpdateLog_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompletionLevel",
                table: "ActivityUpdateLogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "ActivityUpdateLogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletionLevel",
                table: "ActivityUpdateLogs");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "ActivityUpdateLogs");
        }
    }
}
