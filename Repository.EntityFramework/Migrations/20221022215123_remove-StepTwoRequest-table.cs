using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class removeStepTwoRequesttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StepTwoRequests");

            migrationBuilder.AddColumn<string>(
                name: "RequestResponsedBy",
                table: "TripJobsites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RequestStatus",
                table: "TripJobsites",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StageTwoRequestCreatedDate",
                table: "TripJobsites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StageTwoRequestDate",
                table: "TripJobsites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StageTwoResponseDate",
                table: "TripJobsites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestResponsedBy",
                table: "TripJobsites");

            migrationBuilder.DropColumn(
                name: "RequestStatus",
                table: "TripJobsites");

            migrationBuilder.DropColumn(
                name: "StageTwoRequestCreatedDate",
                table: "TripJobsites");

            migrationBuilder.DropColumn(
                name: "StageTwoRequestDate",
                table: "TripJobsites");

            migrationBuilder.DropColumn(
                name: "StageTwoResponseDate",
                table: "TripJobsites");

            migrationBuilder.CreateTable(
                name: "StepTwoRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelted = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TripId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepTwoRequests", x => x.Id);
                });
        }
    }
}
