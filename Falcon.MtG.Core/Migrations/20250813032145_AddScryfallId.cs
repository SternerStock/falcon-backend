using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Falcon.MtG.Migrations
{
    /// <inheritdoc />
    public partial class AddScryfallId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ScryfallId",
                table: "Printings",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScryfallId",
                table: "Printings");
        }
    }
}
