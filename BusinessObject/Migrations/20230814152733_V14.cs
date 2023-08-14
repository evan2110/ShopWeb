using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class V14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupon_Product_product_id",
                table: "Coupon");

            migrationBuilder.DropIndex(
                name: "IX_Coupon_product_id",
                table: "Coupon");

            migrationBuilder.DropColumn(
                name: "product_id",
                table: "Coupon");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "product_id",
                table: "Coupon",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_product_id",
                table: "Coupon",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupon_Product_product_id",
                table: "Coupon",
                column: "product_id",
                principalTable: "Product",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
