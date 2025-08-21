using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IvaCondition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TypeDocuments");

            migrationBuilder.RenameColumn(
                name: "TaxCondition",
                table: "Providers",
                newName: "IvaConditionId");

            migrationBuilder.RenameColumn(
                name: "DocumentType",
                table: "Providers",
                newName: "DocumentTypeId");

            migrationBuilder.RenameColumn(
                name: "TaxCondition",
                table: "Invoices",
                newName: "TaxConditionId");

            migrationBuilder.RenameColumn(
                name: "TaxCondition",
                table: "CommerceDatas",
                newName: "TaxConditionId");

            migrationBuilder.RenameColumn(
                name: "TaxCondition",
                table: "Clients",
                newName: "IvaConditionId");

            migrationBuilder.RenameColumn(
                name: "DocumentType",
                table: "Clients",
                newName: "DocumentTypeId");

            migrationBuilder.AddColumn<int>(
                name: "IvaConditionId",
                table: "Invoices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IvaConditionId",
                table: "CommerceDatas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AfipId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Providers_DocumentTypeId",
                table: "Providers",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_IvaConditionId",
                table: "Providers",
                column: "IvaConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_IvaConditionId",
                table: "Invoices",
                column: "IvaConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_CommerceDatas_IvaConditionId",
                table: "CommerceDatas",
                column: "IvaConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_DocumentTypeId",
                table: "Clients",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_IvaConditionId",
                table: "Clients",
                column: "IvaConditionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_DocumentTypes_DocumentTypeId",
                table: "Clients",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_IvaConditions_IvaConditionId",
                table: "Clients",
                column: "IvaConditionId",
                principalTable: "IvaConditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommerceDatas_IvaConditions_IvaConditionId",
                table: "CommerceDatas",
                column: "IvaConditionId",
                principalTable: "IvaConditions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_IvaConditions_IvaConditionId",
                table: "Invoices",
                column: "IvaConditionId",
                principalTable: "IvaConditions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_DocumentTypes_DocumentTypeId",
                table: "Providers",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_IvaConditions_IvaConditionId",
                table: "Providers",
                column: "IvaConditionId",
                principalTable: "IvaConditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_DocumentTypes_DocumentTypeId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_IvaConditions_IvaConditionId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_CommerceDatas_IvaConditions_IvaConditionId",
                table: "CommerceDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_IvaConditions_IvaConditionId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Providers_DocumentTypes_DocumentTypeId",
                table: "Providers");

            migrationBuilder.DropForeignKey(
                name: "FK_Providers_IvaConditions_IvaConditionId",
                table: "Providers");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropIndex(
                name: "IX_Providers_DocumentTypeId",
                table: "Providers");

            migrationBuilder.DropIndex(
                name: "IX_Providers_IvaConditionId",
                table: "Providers");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_IvaConditionId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_CommerceDatas_IvaConditionId",
                table: "CommerceDatas");

            migrationBuilder.DropIndex(
                name: "IX_Clients_DocumentTypeId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_IvaConditionId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "IvaConditionId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "IvaConditionId",
                table: "CommerceDatas");

            migrationBuilder.RenameColumn(
                name: "IvaConditionId",
                table: "Providers",
                newName: "TaxCondition");

            migrationBuilder.RenameColumn(
                name: "DocumentTypeId",
                table: "Providers",
                newName: "DocumentType");

            migrationBuilder.RenameColumn(
                name: "TaxConditionId",
                table: "Invoices",
                newName: "TaxCondition");

            migrationBuilder.RenameColumn(
                name: "TaxConditionId",
                table: "CommerceDatas",
                newName: "TaxCondition");

            migrationBuilder.RenameColumn(
                name: "IvaConditionId",
                table: "Clients",
                newName: "TaxCondition");

            migrationBuilder.RenameColumn(
                name: "DocumentTypeId",
                table: "Clients",
                newName: "DocumentType");

            migrationBuilder.CreateTable(
                name: "TypeDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AfipId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeDocuments", x => x.Id);
                });
        }
    }
}
