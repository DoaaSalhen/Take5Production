using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class removejobsiteTimingfromtriptable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_JobSites_JobSiteId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_JobSiteId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "ArrivedDate",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "JobSiteId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "StageOneComplatedTime",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "StageTwoComplatedTime",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "departureDate",
                table: "Trips");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivedDate",
                table: "Trips",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "JobSiteId",
                table: "Trips",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "StageOneComplatedTime",
                table: "Trips",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StageTwoComplatedTime",
                table: "Trips",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "departureDate",
                table: "Trips",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
    }
}
