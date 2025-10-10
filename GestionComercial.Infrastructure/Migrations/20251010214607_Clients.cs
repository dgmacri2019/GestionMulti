using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Clients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_SaleConditions_SaleConditionId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_SaleConditionId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "SaleConditionId",
                table: "Clients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SaleConditionId",
                table: "Clients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_SaleConditionId",
                table: "Clients",
                column: "SaleConditionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_SaleConditions_SaleConditionId",
                table: "Clients",
                column: "SaleConditionId",
                principalTable: "SaleConditions",
                principalColumn: "Id");
        }
    }
}
