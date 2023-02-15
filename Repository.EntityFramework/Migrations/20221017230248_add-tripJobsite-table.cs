using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class addtripJobsitetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TripJobsites",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<long>(type: "bigint", nullable: false),
                    JobSiteId = table.Column<long>(type: "bigint", nullable: false),
                    StartingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DestinationArrivingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StageOneComplatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StageTwoComplatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Take5Status = table.Column<int>(type: "int", nullable: false),
                    TripStatus = table.Column<int>(type: "int", nullable: false),
                    IsDelted = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripJobsites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripJobsites_JobSites_JobSiteId",
                        column: x => x.JobSiteId,
                        principalTable: "JobSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TripJobsites_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TripJobsites_JobSiteId",
                table: "TripJobsites",
                column: "JobSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_TripJobsites_TripId",
                table: "TripJobsites",
                column: "TripId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TripJobsites");
        }
    }
}
