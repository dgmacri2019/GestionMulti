using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CommerceDataModification1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommerceDatas_IvaConditions_IvaConditionId",
                table: "CommerceDatas");

            migrationBuilder.DropColumn(
                name: "TaxConditionId",
                table: "CommerceDatas");

            migrationBuilder.AlterColumn<int>(
                name: "IvaConditionId",
                table: "CommerceDatas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CommerceDatas_IvaConditions_IvaConditionId",
                table: "CommerceDatas",
                column: "IvaConditionId",
                principalTable: "IvaConditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommerceDatas_IvaConditions_IvaConditionId",
                table: "CommerceDatas");

            migrationBuilder.AlterColumn<int>(
                name: "IvaConditionId",
                table: "CommerceDatas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TaxConditionId",
                table: "CommerceDatas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_CommerceDatas_IvaConditions_IvaConditionId",
                table: "CommerceDatas",
                column: "IvaConditionId",
                principalTable: "IvaConditions",
                principalColumn: "Id");
        }
    }
}
