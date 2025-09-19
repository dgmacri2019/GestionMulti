using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Invoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_IvaConditions_IvaConditionId",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "TaxConditionId",
                table: "Invoices",
                newName: "ReceptorIvaId");

            migrationBuilder.AlterColumn<int>(
                name: "IvaConditionId",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceDate",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_IvaConditions_IvaConditionId",
                table: "Invoices",
                column: "IvaConditionId",
                principalTable: "IvaConditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_IvaConditions_IvaConditionId",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "ReceptorIvaId",
                table: "Invoices",
                newName: "TaxConditionId");

            migrationBuilder.AlterColumn<int>(
                name: "IvaConditionId",
                table: "Invoices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceDate",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_IvaConditions_IvaConditionId",
                table: "Invoices",
                column: "IvaConditionId",
                principalTable: "IvaConditions",
                principalColumn: "Id");
        }
    }
}
