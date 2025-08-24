using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ClientAndProviderOptionalCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OptionalCode",
                table: "Providers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptionalCode",
                table: "Clients",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "Provider_Code_Index",
                table: "Providers",
                column: "OptionalCode",
                unique: true,
                filter: "[OptionalCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "Provider_Document_Index",
                table: "Providers",
                columns: new[] { "DocumentNumber", "DocumentTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Client_Code_Index",
                table: "Clients",
                column: "OptionalCode",
                unique: true,
                filter: "[OptionalCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "Client_Document_Index",
                table: "Clients",
                columns: new[] { "DocumentNumber", "DocumentTypeId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Provider_Code_Index",
                table: "Providers");

            migrationBuilder.DropIndex(
                name: "Provider_Document_Index",
                table: "Providers");

            migrationBuilder.DropIndex(
                name: "Client_Code_Index",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "Client_Document_Index",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "OptionalCode",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "OptionalCode",
                table: "Clients");
        }
    }
}
