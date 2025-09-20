using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Invoice1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Invoices_SaleId",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "DocType",
                table: "Invoices",
                newName: "Concepto");

            migrationBuilder.RenameColumn(
                name: "DocNro",
                table: "Invoices",
                newName: "ClientDocNro");

            migrationBuilder.AddColumn<int>(
                name: "ClientDocType",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "InternalTax",
                table: "Invoices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "UseHomologacion",
                table: "Billings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "Invoice_SaleId-CompTypeId_Index",
                table: "Invoices",
                columns: new[] { "SaleId", "CompTypeId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Invoice_SaleId-CompTypeId_Index",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "ClientDocType",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "InternalTax",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "UseHomologacion",
                table: "Billings");

            migrationBuilder.RenameColumn(
                name: "Concepto",
                table: "Invoices",
                newName: "DocType");

            migrationBuilder.RenameColumn(
                name: "ClientDocNro",
                table: "Invoices",
                newName: "DocNro");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SaleId",
                table: "Invoices",
                column: "SaleId");
        }
    }
}
