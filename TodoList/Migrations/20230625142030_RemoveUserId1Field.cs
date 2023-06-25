using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoList.Migrations
{
    public partial class RemoveUserId1Field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItem_User_UserId1",
                table: "TodoItem");

            migrationBuilder.DropIndex(
                name: "IX_TodoItem_UserId1",
                table: "TodoItem");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "TodoItem");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "TodoItem",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItem_UserId",
                table: "TodoItem",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItem_User_UserId",
                table: "TodoItem",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItem_User_UserId",
                table: "TodoItem");

            migrationBuilder.DropIndex(
                name: "IX_TodoItem_UserId",
                table: "TodoItem");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TodoItem",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "TodoItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TodoItem_UserId1",
                table: "TodoItem",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItem_User_UserId1",
                table: "TodoItem",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
