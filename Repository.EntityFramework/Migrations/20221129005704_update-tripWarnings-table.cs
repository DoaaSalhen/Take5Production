using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class updatetripWarningstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<long>(
                name: "TripJobsiteTripId",
                table: "TripJobsiteWarnings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TripJobsiteJobSiteId",
                table: "TripJobsiteWarnings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_TripJobsiteWarnings_TripJobsites_TripJobsiteTripId_TripJobsiteJobSiteId",
                table: "TripJobsiteWarnings",
                columns: new[] { "TripJobsiteTripId", "TripJobsiteJobSiteId" },
                principalTable: "TripJobsites",
                principalColumns: new[] { "TripId", "JobSiteId" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripJobsiteWarnings_TripJobsites_TripJobsiteTripId_TripJobsiteJobSiteId",
                table: "TripJobsiteWarnings");

            migrationBuilder.DropColumn(
                name: "TripJobsiteTripId",
                table: "TripJobsiteWarnings");

            migrationBuilder.AlterColumn<long>(
                name: "TripJobsiteJobSiteId",
                table: "TripJobsiteWarnings");


            migrationBuilder.AddForeignKey(
                name: "FK_TripJobsiteWarnings_TripJobsites_TripJobsiteTripId_TripJobsiteJobSiteId",
                table: "TripJobsiteWarnings",
                columns: new[] { "TripJobsiteTripId", "TripJobsiteJobSiteId" },
                principalTable: "TripJobsites",
                principalColumns: new[] { "TripId", "JobSiteId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
