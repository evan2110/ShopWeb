using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class V13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "freight",
                table: "Order");

            migrationBuilder.AddColumn<string>(
                name: "ship_phone",
                table: "Order",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ship_phone",
                table: "Order");

            migrationBuilder.AddColumn<decimal>(
                name: "freight",
                table: "Order",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
