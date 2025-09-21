using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class pcParameter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogin",
                table: "PcParameters",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLogin",
                table: "PcParameters");
        }
    }
}
