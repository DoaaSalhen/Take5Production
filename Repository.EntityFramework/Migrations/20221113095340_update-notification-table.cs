using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class updatenotificationtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "JobSiteId",
                table: "Notifications",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TripId",
                table: "Notifications",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_JobSiteId",
                table: "Notifications",
                column: "JobSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_TripId",
                table: "Notifications",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_JobSites_JobSiteId",
                table: "Notifications",
                column: "JobSiteId",
                principalTable: "JobSites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Trips_TripId",
                table: "Notifications",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_JobSites_JobSiteId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Trips_TripId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_JobSiteId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_TripId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "JobSiteId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "Notifications");
        }
    }
}
