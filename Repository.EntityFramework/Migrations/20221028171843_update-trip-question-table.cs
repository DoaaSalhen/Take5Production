using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class updatetripquestiontable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TripQuestions",
                table: "TripQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TripDangers",
                table: "TripDangers");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "TripQuestions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "TripDangers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripQuestions",
                table: "TripQuestions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripDangers",
                table: "TripDangers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TripQuestions_TripId",
                table: "TripQuestions",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_TripDangers_TripId",
                table: "TripDangers",
                column: "TripId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TripQuestions",
                table: "TripQuestions");

            migrationBuilder.DropIndex(
                name: "IX_TripQuestions_TripId",
                table: "TripQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TripDangers",
                table: "TripDangers");

            migrationBuilder.DropIndex(
                name: "IX_TripDangers_TripId",
                table: "TripDangers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TripQuestions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TripDangers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripQuestions",
                table: "TripQuestions",
                columns: new[] { "TripId", "QuestionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripDangers",
                table: "TripDangers",
                columns: new[] { "TripId", "MeasureControlId" });
        }
    }
}
