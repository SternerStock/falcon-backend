using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Falcon.MtG.Migrations
{
    public partial class v5_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UUID",
                table: "Cards");

            migrationBuilder.AddColumn<Guid>(
                name: "UUID",
                table: "Printings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UUID",
                table: "Printings");

            migrationBuilder.AddColumn<Guid>(
                name: "UUID",
                table: "Cards",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
