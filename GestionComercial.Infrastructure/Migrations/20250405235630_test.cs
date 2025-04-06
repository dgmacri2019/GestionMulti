using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Product_BarCode_Index",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "Product_BarCode_Index",
                table: "Products",
                column: "BarCode",
                unique: true,
                filter: "[BarCode] IS NOT NULL AND [BarCode] <> ''");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Product_BarCode_Index",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "Product_BarCode_Index",
                table: "Products",
                column: "BarCode",
                unique: true,
                filter: "[BarCode] IS NOT NULL");
        }
    }
}
