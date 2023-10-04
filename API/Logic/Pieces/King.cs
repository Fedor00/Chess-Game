using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Logic
{
    public class King : Piece
    {
        public King(Color color, Position position) : base(color, position)
        {
        }
        public override char Symbol => 'K';
        public override List<Position> PotentialCaptures(ChessBoard board)
        {
            return ValidMoves(board).Where(move => board[move.Rank, move.File] != null).ToList();
        }
        public override List<Position> ValidMoves(ChessBoard board)
        {
            List<Position> moves = new();

            int[] dX = { 1, 1, -1, -1, 0, 0, 1, -1 };
            int[] dY = { 1, -1, 1, -1, 1, -1, 0, 0 };

            for (int i = 0; i < 8; i++)
            {
                Position newPos = new(Position.Rank + dX[i], Position.File + dY[i]);
                if (newPos.Rank >= 0 && newPos.Rank < 8 && newPos.File >= 0 && newPos.File < 8)
                {
                    Piece occupyingPiece = board[newPos.Rank, newPos.File];
                    if (occupyingPiece == null || occupyingPiece.PieceColor != this.PieceColor)
                    {
                        moves.Add(newPos);
                    }
                }
            }

            return moves;
        }

    }
}