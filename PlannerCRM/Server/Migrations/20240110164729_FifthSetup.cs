using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlannerCRM.Server.Migrations
{
    /// <inheritdoc />
    public partial class FifthSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfilePictures_Employees_EmployeeInfoId",
                table: "ProfilePictures");

            migrationBuilder.DropIndex(
                name: "IX_ProfilePictures_EmployeeInfoId",
                table: "ProfilePictures");

            migrationBuilder.DropColumn(
                name: "EmployeeInfoId",
                table: "ProfilePictures");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "ProfilePictures",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePictures_EmployeeId",
                table: "ProfilePictures",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfilePictures_Employees_EmployeeId",
                table: "ProfilePictures",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfilePictures_Employees_EmployeeId",
                table: "ProfilePictures");

            migrationBuilder.DropIndex(
                name: "IX_ProfilePictures_EmployeeId",
                table: "ProfilePictures");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "ProfilePictures",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeInfoId",
                table: "ProfilePictures",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePictures_EmployeeInfoId",
                table: "ProfilePictures",
                column: "EmployeeInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfilePictures_Employees_EmployeeInfoId",
                table: "ProfilePictures",
                column: "EmployeeInfoId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
