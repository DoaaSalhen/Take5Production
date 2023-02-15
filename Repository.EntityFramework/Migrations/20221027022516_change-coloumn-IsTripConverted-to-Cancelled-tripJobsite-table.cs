using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class changecoloumnIsTripConvertedtoCancelledtripJobsitetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsTripConverted",
                table: "TripJobsites",
                newName: "Cancelled");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cancelled",
                table: "TripJobsites",
                newName: "IsTripConverted");
        }
    }
}
