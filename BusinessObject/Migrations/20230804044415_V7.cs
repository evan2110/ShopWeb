using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class V7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "color_id",
                table: "CartItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "size_id",
                table: "CartItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_color_id",
                table: "CartItem",
                column: "color_id");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_size_id",
                table: "CartItem",
                column: "size_id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_Color_color_id",
                table: "CartItem",
                column: "color_id",
                principalTable: "Color",
                principalColumn: "color_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_Size_size_id",
                table: "CartItem",
                column: "size_id",
                principalTable: "Size",
                principalColumn: "size_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_Color_color_id",
                table: "CartItem");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_Size_size_id",
                table: "CartItem");

            migrationBuilder.DropIndex(
                name: "IX_CartItem_color_id",
                table: "CartItem");

            migrationBuilder.DropIndex(
                name: "IX_CartItem_size_id",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "color_id",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "size_id",
                table: "CartItem");
        }
    }
}
