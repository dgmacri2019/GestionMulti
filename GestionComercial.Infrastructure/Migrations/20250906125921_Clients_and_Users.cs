using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Clients_and_Users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_SaleConditions_SaleConditionId",
                table: "Clients");

            migrationBuilder.AlterColumn<decimal>(
                name: "Sold",
                table: "Clients",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4);

            migrationBuilder.AlterColumn<int>(
                name: "SaleConditionId",
                table: "Clients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "CreditLimit",
                table: "Clients",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_SaleConditions_SaleConditionId",
                table: "Clients",
                column: "SaleConditionId",
                principalTable: "SaleConditions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_SaleConditions_SaleConditionId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CreditLimit",
                table: "Clients");

            migrationBuilder.AlterColumn<decimal>(
                name: "Sold",
                table: "Clients",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "SaleConditionId",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_SaleConditions_SaleConditionId",
                table: "Clients",
                column: "SaleConditionId",
                principalTable: "SaleConditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
