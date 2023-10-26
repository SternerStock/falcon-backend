using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Falcon.MtG.Migrations
{
    /// <inheritdoc />
    public partial class SaltRank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "EDHRECSalt",
                table: "Cards",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EDHRECSalt",
                table: "Cards");
        }
    }
}
