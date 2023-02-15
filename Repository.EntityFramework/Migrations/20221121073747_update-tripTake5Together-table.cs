using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class updatetripTake5Togethertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripTake5Togethers_Drivers_DriverId",
                table: "TripTake5Togethers");

            migrationBuilder.DropIndex(
                name: "IX_TripTake5Togethers_DriverId",
                table: "TripTake5Togethers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TripTake5Togethers_DriverId",
                table: "TripTake5Togethers",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_TripTake5Togethers_Drivers_DriverId",
                table: "TripTake5Togethers",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
