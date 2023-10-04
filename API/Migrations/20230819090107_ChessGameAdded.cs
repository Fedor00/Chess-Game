using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class ChessGameAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChessGames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TopPlayerId = table.Column<int>(type: "integer", nullable: false),
                    BottomPlayerId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CurrentFEN = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChessGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChessGames_Users_BottomPlayerId",
                        column: x => x.BottomPlayerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChessGames_Users_TopPlayerId",
                        column: x => x.TopPlayerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChessGames_BottomPlayerId",
                table: "ChessGames",
                column: "BottomPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChessGames_TopPlayerId",
                table: "ChessGames",
                column: "TopPlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChessGames");
        }
    }
}
