using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Sales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_SaleConditions_SaleConditionId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_SaleConditionId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "SaleConditionId",
                table: "Sales");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SaleConditionId",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_SaleConditionId",
                table: "Sales",
                column: "SaleConditionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_SaleConditions_SaleConditionId",
                table: "Sales",
                column: "SaleConditionId",
                principalTable: "SaleConditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
