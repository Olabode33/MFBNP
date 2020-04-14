using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PMSDemo.Migrations
{
    public partial class Added_PMS_Entities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AbpOrganizationUnits",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MdaId",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PerformanceActivitys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    OrganizationUnitId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    MilestoneAchieved = table.Column<string>(nullable: true),
                    PlannedStartDate = table.Column<DateTime>(nullable: false),
                    ActualStartDate = table.Column<DateTime>(nullable: false),
                    PlannedCompletionDate = table.Column<DateTime>(nullable: false),
                    ActualCompletionDate = table.Column<DateTime>(nullable: false),
                    CompletionStatus = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceActivitys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PerformanceIndicators",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    OrganizationUnitId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    BaselineValue = table.Column<string>(nullable: true),
                    BaselineComment = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Target = table.Column<string>(nullable: true),
                    Actual = table.Column<string>(nullable: true),
                    DataType = table.Column<int>(nullable: false),
                    Unit = table.Column<int>(nullable: false),
                    ComparisonMethod = table.Column<int>(nullable: false),
                    MeansOfVerification = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceIndicators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PerformanceReviews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    OrganizationUnitId = table.Column<long>(nullable: false),
                    ReviewComment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceReviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriorityAreas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriorityAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivityUpdateLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    PerformanceActivityId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityUpdateLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityUpdateLogs_PerformanceActivitys_PerformanceActivityId",
                        column: x => x.PerformanceActivityId,
                        principalTable: "PerformanceActivitys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndicatorUpdateLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    PerformanceIndicatorId = table.Column<int>(nullable: false),
                    ActualValue = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicatorUpdateLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndicatorUpdateLogs_PerformanceIndicators_PerformanceIndicatorId",
                        column: x => x.PerformanceIndicatorId,
                        principalTable: "PerformanceIndicators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_MdaId",
                table: "AbpOrganizationUnits",
                column: "MdaId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityUpdateLogs_PerformanceActivityId",
                table: "ActivityUpdateLogs",
                column: "PerformanceActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_IndicatorUpdateLogs_PerformanceIndicatorId",
                table: "IndicatorUpdateLogs",
                column: "PerformanceIndicatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpOrganizationUnits_AbpOrganizationUnits_MdaId",
                table: "AbpOrganizationUnits",
                column: "MdaId",
                principalTable: "AbpOrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpOrganizationUnits_AbpOrganizationUnits_MdaId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropTable(
                name: "ActivityUpdateLogs");

            migrationBuilder.DropTable(
                name: "IndicatorUpdateLogs");

            migrationBuilder.DropTable(
                name: "PerformanceReviews");

            migrationBuilder.DropTable(
                name: "PriorityAreas");

            migrationBuilder.DropTable(
                name: "PerformanceActivitys");

            migrationBuilder.DropTable(
                name: "PerformanceIndicators");

            migrationBuilder.DropIndex(
                name: "IX_AbpOrganizationUnits_MdaId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "MdaId",
                table: "AbpOrganizationUnits");
        }
    }
}
