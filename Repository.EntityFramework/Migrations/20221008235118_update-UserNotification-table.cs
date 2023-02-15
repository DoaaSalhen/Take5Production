using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class updateUserNotificationtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "StepTwoRequests",
                newName: "ApprovalDate");

            migrationBuilder.AddColumn<bool>(
                name: "Seen",
                table: "UserNotifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "TripStatus",
                table: "Trips",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "Take5Status",
                table: "Trips",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "StepTwoRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seen",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StepTwoRequests");

            migrationBuilder.RenameColumn(
                name: "ApprovalDate",
                table: "StepTwoRequests",
                newName: "UpdatedDate");

            migrationBuilder.AlterColumn<bool>(
                name: "TripStatus",
                table: "Trips",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "Take5Status",
                table: "Trips",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
