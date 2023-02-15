using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class updateTriptable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "JobSiteId",
                table: "Trips",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_JobSiteId",
                table: "Trips",
                column: "JobSiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_JobSites_JobSiteId",
                table: "Trips",
                column: "JobSiteId",
                principalTable: "JobSites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_JobSites_JobSiteId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_JobSiteId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "JobSiteId",
                table: "Trips");
        }
    }
}
