using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class updateJobSitetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cordinates",
                table: "JobSites");

            migrationBuilder.AddColumn<string>(
                name: "Desc",
                table: "JobSites",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "JobSites",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "JobSites",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Desc",
                table: "JobSites");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "JobSites");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "JobSites");

            migrationBuilder.AddColumn<string>(
                name: "Cordinates",
                table: "JobSites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
