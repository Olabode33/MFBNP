using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PMSDemo.Migrations
{
    public partial class Addedattachmententitiesforindicatorandactivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompletionLevel",
                table: "PerformanceActivities",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ActivityAttachments",
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
                    FileName = table.Column<string>(nullable: true),
                    DocumentId = table.Column<Guid>(nullable: true),
                    FileFormat = table.Column<string>(nullable: true),
                    PerformanceActivityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityAttachments_AppBinaryObjects_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "AppBinaryObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityAttachments_PerformanceActivities_PerformanceActivityId",
                        column: x => x.PerformanceActivityId,
                        principalTable: "PerformanceActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndicatorAttachments",
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
                    FileName = table.Column<string>(nullable: true),
                    DocumentId = table.Column<Guid>(nullable: true),
                    FileFormat = table.Column<string>(nullable: true),
                    PerformanceIndicatorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicatorAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndicatorAttachments_AppBinaryObjects_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "AppBinaryObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IndicatorAttachments_PerformanceIndicators_PerformanceIndicatorId",
                        column: x => x.PerformanceIndicatorId,
                        principalTable: "PerformanceIndicators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAttachments_DocumentId",
                table: "ActivityAttachments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAttachments_PerformanceActivityId",
                table: "ActivityAttachments",
                column: "PerformanceActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_IndicatorAttachments_DocumentId",
                table: "IndicatorAttachments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_IndicatorAttachments_PerformanceIndicatorId",
                table: "IndicatorAttachments",
                column: "PerformanceIndicatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityAttachments");

            migrationBuilder.DropTable(
                name: "IndicatorAttachments");

            migrationBuilder.DropColumn(
                name: "CompletionLevel",
                table: "PerformanceActivities");
        }
    }
}
