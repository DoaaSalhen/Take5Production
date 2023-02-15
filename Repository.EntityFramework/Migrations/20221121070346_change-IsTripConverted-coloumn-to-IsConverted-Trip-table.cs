using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class changeIsTripConvertedcoloumntoIsConvertedTriptable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsTripConverted",
                table: "Trips",
                newName: "IsConverted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsConverted",
                table: "Trips",
                newName: "IsTripConverted");
        }
    }
}
