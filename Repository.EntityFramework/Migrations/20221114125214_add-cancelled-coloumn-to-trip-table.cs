using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class addcancelledcoloumntotriptable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cancelled",
                table: "TripJobsites");

            migrationBuilder.AddColumn<bool>(
                name: "Cancelled",
                table: "Trips",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cancelled",
                table: "Trips");

            migrationBuilder.AddColumn<bool>(
                name: "Cancelled",
                table: "TripJobsites",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
