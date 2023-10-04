using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class TopPlayerNullableCorrectlyDone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChessGames_Users_BottomPlayerId",
                table: "ChessGames");

            migrationBuilder.AlterColumn<int>(
                name: "TopPlayerId",
                table: "ChessGames",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "BottomPlayerId",
                table: "ChessGames",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_ChessGames_Users_BottomPlayerId",
                table: "ChessGames",
                column: "BottomPlayerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChessGames_Users_BottomPlayerId",
                table: "ChessGames");

            migrationBuilder.AlterColumn<int>(
                name: "TopPlayerId",
                table: "ChessGames",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BottomPlayerId",
                table: "ChessGames",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChessGames_Users_BottomPlayerId",
                table: "ChessGames",
                column: "BottomPlayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
