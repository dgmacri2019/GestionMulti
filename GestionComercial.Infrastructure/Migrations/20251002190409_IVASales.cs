using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IVASales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BaseImp0",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseImp25",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseImp5",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalIVA25",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalIVA5",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseImp0",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "BaseImp25",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "BaseImp5",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "TotalIVA25",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "TotalIVA5",
                table: "Sales");
        }
    }
}
