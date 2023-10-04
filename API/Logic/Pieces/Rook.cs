using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Logic
{
    public class Rook : Piece
    {
        public Rook(Color color, Position position) : base(color, position)
        {
        }

        public override char Symbol => 'R';

        public override List<Position> PotentialCaptures(ChessBoard board)
        {
            return ValidMoves(board).Where(move => board[move.Rank, move.File] != null).ToList();
        }

        public override List<Position> ValidMoves(ChessBoard board)
        {
            List<Position> validMoves = new();
            // Horizontal and vertical moves
            validMoves.AddRange(GetMovesInDirection(board, Position, 0, 1));  // right
            validMoves.AddRange(GetMovesInDirection(board, Position, 0, -1)); // left
            validMoves.AddRange(GetMovesInDirection(board, Position, 1, 0));  // up
            validMoves.AddRange(GetMovesInDirection(board, Position, -1, 0)); // down
            return validMoves;
        }
    }
}