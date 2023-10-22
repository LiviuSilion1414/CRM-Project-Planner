using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlannerCRM.Server.Migrations
{
    /// <inheritdoc />
    public partial class Secundary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientWorkOrders_Clients_WorkOrderId",
                table: "ClientWorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_ClientWorkOrders_WorkOrderId",
                table: "ClientWorkOrders");

            migrationBuilder.AddColumn<int>(
                name: "FirmClientId",
                table: "ClientWorkOrders",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientWorkOrders_FirmClientId",
                table: "ClientWorkOrders",
                column: "FirmClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientWorkOrders_WorkOrderId",
                table: "ClientWorkOrders",
                column: "WorkOrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientWorkOrders_Clients_FirmClientId",
                table: "ClientWorkOrders",
                column: "FirmClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientWorkOrders_WorkOrders_WorkOrderId",
                table: "ClientWorkOrders",
                column: "WorkOrderId",
                principalTable: "WorkOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientWorkOrders_Clients_FirmClientId",
                table: "ClientWorkOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientWorkOrders_WorkOrders_WorkOrderId",
                table: "ClientWorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_ClientWorkOrders_FirmClientId",
                table: "ClientWorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_ClientWorkOrders_WorkOrderId",
                table: "ClientWorkOrders");

            migrationBuilder.DropColumn(
                name: "FirmClientId",
                table: "ClientWorkOrders");

            migrationBuilder.CreateIndex(
                name: "IX_ClientWorkOrders_WorkOrderId",
                table: "ClientWorkOrders",
                column: "WorkOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientWorkOrders_Clients_WorkOrderId",
                table: "ClientWorkOrders",
                column: "WorkOrderId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
