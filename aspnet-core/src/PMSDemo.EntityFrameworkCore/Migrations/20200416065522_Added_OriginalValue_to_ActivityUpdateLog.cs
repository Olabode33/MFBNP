using Microsoft.EntityFrameworkCore.Migrations;

namespace PMSDemo.Migrations
{
    public partial class Added_OriginalValue_to_ActivityUpdateLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OriginalValue",
                table: "ActivityUpdateLogs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalValue",
                table: "ActivityUpdateLogs");
        }
    }
}
