using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class TopPlayerNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChessGames_Users_TopPlayerId",
                table: "ChessGames");

            migrationBuilder.AddForeignKey(
                name: "FK_ChessGames_Users_TopPlayerId",
                table: "ChessGames",
                column: "TopPlayerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChessGames_Users_TopPlayerId",
                table: "ChessGames");

            migrationBuilder.AddForeignKey(
                name: "FK_ChessGames_Users_TopPlayerId",
                table: "ChessGames",
                column: "TopPlayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
