using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class ChangetripjobsiteprimaryKeycoloumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TripJobsites",
                table: "TripJobsites");

            migrationBuilder.DropIndex(
                name: "IX_TripJobsites_TripId",
                table: "TripJobsites");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TripJobsites");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripJobsites",
                table: "TripJobsites",
                columns: new[] { "TripId", "JobSiteId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TripJobsites",
                table: "TripJobsites");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "TripJobsites",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripJobsites",
                table: "TripJobsites",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TripJobsites_TripId",
                table: "TripJobsites",
                column: "TripId");
        }
    }
}
