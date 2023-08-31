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
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "WorkOrderCosts");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "WorkOrderCosts",
                newName: "IsCreated");

            migrationBuilder.AddColumn<bool>(
                name: "IsInvoiceCreated",
                table: "WorkOrders",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInvoiceCreated",
                table: "WorkOrders");

            migrationBuilder.RenameColumn(
                name: "IsCreated",
                table: "WorkOrderCosts",
                newName: "IsDeleted");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "WorkOrderCosts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
