using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class accounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Account_AccountNumber_Index",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_AccountTypeId",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "AccountGroupNumber",
                table: "Accounts",
                newName: "AccountIdSubGroupNumber4");

            migrationBuilder.AddColumn<int>(
                name: "AccountIdSubGroupNumber1",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccountIdSubGroupNumber2",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccountIdSubGroupNumber3",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ForeignCurrency",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "Account_AccountNumber_Index",
                table: "Accounts",
                columns: new[] { "AccountTypeId", "AccountSubGroupNumber1", "AccountSubGroupNumber2", "AccountSubGroupNumber3", "AccountSubGroupNumber4", "AccountSubGroupNumber5" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Account_AccountNumber_Index",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccountIdSubGroupNumber1",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccountIdSubGroupNumber2",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccountIdSubGroupNumber3",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ForeignCurrency",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "AccountIdSubGroupNumber4",
                table: "Accounts",
                newName: "AccountGroupNumber");

            migrationBuilder.CreateIndex(
                name: "Account_AccountNumber_Index",
                table: "Accounts",
                columns: new[] { "AccountGroupNumber", "AccountSubGroupNumber1", "AccountSubGroupNumber2", "AccountSubGroupNumber3", "AccountSubGroupNumber4", "AccountSubGroupNumber5" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountTypeId",
                table: "Accounts",
                column: "AccountTypeId");
        }
    }
}
