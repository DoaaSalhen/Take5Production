using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class updatetripDanger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripDangers_Dangers_DangerId",
                table: "TripDangers");

            migrationBuilder.DropTable(
                name: "TripDangerControls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TripDangers",
                table: "TripDangers");

            migrationBuilder.DropIndex(
                name: "IX_TripDangers_DangerId",
                table: "TripDangers");

            migrationBuilder.DropColumn(
                name: "DangerId",
                table: "TripDangers");

            migrationBuilder.AddColumn<long>(
                name: "MeasureControlId",
                table: "TripDangers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripDangers",
                table: "TripDangers",
                columns: new[] { "TripId", "MeasureControlId" });

            migrationBuilder.CreateIndex(
                name: "IX_TripDangers_MeasureControlId",
                table: "TripDangers",
                column: "MeasureControlId");

            migrationBuilder.AddForeignKey(
                name: "FK_TripDangers_MeasureControls_MeasureControlId",
                table: "TripDangers",
                column: "MeasureControlId",
                principalTable: "MeasureControls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripDangers_MeasureControls_MeasureControlId",
                table: "TripDangers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TripDangers",
                table: "TripDangers");

            migrationBuilder.DropIndex(
                name: "IX_TripDangers_MeasureControlId",
                table: "TripDangers");

            migrationBuilder.DropColumn(
                name: "MeasureControlId",
                table: "TripDangers");

            migrationBuilder.AddColumn<int>(
                name: "DangerId",
                table: "TripDangers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripDangers",
                table: "TripDangers",
                columns: new[] { "TripId", "DangerId" });

            migrationBuilder.CreateTable(
                name: "TripDangerControls",
                columns: table => new
                {
                    TripId = table.Column<long>(type: "bigint", nullable: false),
                    DangerId = table.Column<int>(type: "int", nullable: false),
                    ControlMeasure = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripDangerControls", x => new { x.TripId, x.DangerId });
                    table.ForeignKey(
                        name: "FK_TripDangerControls_Dangers_DangerId",
                        column: x => x.DangerId,
                        principalTable: "Dangers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TripDangerControls_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TripDangers_DangerId",
                table: "TripDangers",
                column: "DangerId");

            migrationBuilder.CreateIndex(
                name: "IX_TripDangerControls_DangerId",
                table: "TripDangerControls",
                column: "DangerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TripDangers_Dangers_DangerId",
                table: "TripDangers",
                column: "DangerId",
                principalTable: "Dangers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
