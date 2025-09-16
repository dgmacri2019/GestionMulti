using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CommerceDataModification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommerceDatas_Cities_CityId",
                table: "CommerceDatas");

            migrationBuilder.DropIndex(
                name: "IX_CommerceDatas_CityId",
                table: "CommerceDatas");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "CommerceDatas");

            migrationBuilder.DropColumn(
                name: "PadronA5ExpirationTime",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "PadronA5GenerationTime",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "PadronA5Sign",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "PadronA5Token",
                table: "Billings");

            migrationBuilder.AddColumn<int>(
                name: "CommerceDataId",
                table: "States",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommerceDataId",
                table: "IvaConditions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "CommerceDatas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_States_CommerceDataId",
                table: "States",
                column: "CommerceDataId");

            migrationBuilder.CreateIndex(
                name: "IX_IvaConditions_CommerceDataId",
                table: "IvaConditions",
                column: "CommerceDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_IvaConditions_CommerceDatas_CommerceDataId",
                table: "IvaConditions",
                column: "CommerceDataId",
                principalTable: "CommerceDatas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_States_CommerceDatas_CommerceDataId",
                table: "States",
                column: "CommerceDataId",
                principalTable: "CommerceDatas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IvaConditions_CommerceDatas_CommerceDataId",
                table: "IvaConditions");

            migrationBuilder.DropForeignKey(
                name: "FK_States_CommerceDatas_CommerceDataId",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_States_CommerceDataId",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_IvaConditions_CommerceDataId",
                table: "IvaConditions");

            migrationBuilder.DropColumn(
                name: "CommerceDataId",
                table: "States");

            migrationBuilder.DropColumn(
                name: "CommerceDataId",
                table: "IvaConditions");

            migrationBuilder.DropColumn(
                name: "City",
                table: "CommerceDatas");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "CommerceDatas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PadronA5ExpirationTime",
                table: "Billings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PadronA5GenerationTime",
                table: "Billings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PadronA5Sign",
                table: "Billings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PadronA5Token",
                table: "Billings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CommerceDatas_CityId",
                table: "CommerceDatas",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommerceDatas_Cities_CityId",
                table: "CommerceDatas",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
