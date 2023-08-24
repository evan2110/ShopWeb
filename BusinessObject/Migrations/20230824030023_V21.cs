using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class V21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Support_User_UserId",
                table: "Support");

            migrationBuilder.DropIndex(
                name: "IX_Support_UserId",
                table: "Support");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Support");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Support",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Support_UserId",
                table: "Support",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Support_User_UserId",
                table: "Support",
                column: "UserId",
                principalTable: "User",
                principalColumn: "user_id");
        }
    }
}
