using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlannerCRM.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkTimeRecords_AspNetUsers_EmployeeId",
                schema: "public",
                table: "WorkTimeRecords");

            migrationBuilder.DropIndex(
                name: "IX_WorkTimeRecords_EmployeeId",
                schema: "public",
                table: "WorkTimeRecords");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WorkTimeRecords_EmployeeId",
                schema: "public",
                table: "WorkTimeRecords",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTimeRecords_AspNetUsers_EmployeeId",
                schema: "public",
                table: "WorkTimeRecords",
                column: "EmployeeId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
