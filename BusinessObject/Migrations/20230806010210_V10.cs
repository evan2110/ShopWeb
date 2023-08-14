using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class V10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_ShipVia_ship_via_id",
                table: "Order");

            migrationBuilder.DropTable(
                name: "ShipVia");

            migrationBuilder.DropIndex(
                name: "IX_Order_ship_via_id",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ship_city",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ship_country",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ship_name",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ship_postal_code",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ship_region",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ship_via_id",
                table: "Order");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ship_city",
                table: "Order",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ship_country",
                table: "Order",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ship_name",
                table: "Order",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ship_postal_code",
                table: "Order",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ship_region",
                table: "Order",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ship_via_id",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ShipVia",
                columns: table => new
                {
                    ship_via_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    created_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    ship_via_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    updated_time = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipVia", x => x.ship_via_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_ship_via_id",
                table: "Order",
                column: "ship_via_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_ShipVia_ship_via_id",
                table: "Order",
                column: "ship_via_id",
                principalTable: "ShipVia",
                principalColumn: "ship_via_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
