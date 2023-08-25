using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class V22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Support_Room_room_id",
                table: "Support");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.RenameColumn(
                name: "room_id",
                table: "Support",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_Support_room_id",
                table: "Support",
                newName: "IX_Support_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Support_User_user_id",
                table: "Support",
                column: "user_id",
                principalTable: "User",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Support_User_user_id",
                table: "Support");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Support",
                newName: "room_id");

            migrationBuilder.RenameIndex(
                name: "IX_Support_user_id",
                table: "Support",
                newName: "IX_Support_room_id");

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    room_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    created_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    updated_time = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.room_id);
                    table.ForeignKey(
                        name: "FK_Room_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Room_user_id",
                table: "Room",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Support_Room_room_id",
                table: "Support",
                column: "room_id",
                principalTable: "Room",
                principalColumn: "room_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
