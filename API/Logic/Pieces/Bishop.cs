using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Logic
{
    public class Bishop : Piece
    {
        public Bishop(Color color, Position position) : base(color, position)
        {
        }

        public override char Symbol => 'B';

        public override List<Position> PotentialCaptures(ChessBoard board)
        {
            return ValidMoves(board).Where(move => board[move.Rank, move.File] != null).ToList();
        }

        public override List<Position> ValidMoves(ChessBoard board)
        {
            List<Position> validMoves = new();
            validMoves.AddRange(GetMovesInDirection(board, Position, 1, 1));  // upper-right
            validMoves.AddRange(GetMovesInDirection(board, Position, 1, -1)); // upper-left
            validMoves.AddRange(GetMovesInDirection(board, Position, -1, 1)); // lower-right
            validMoves.AddRange(GetMovesInDirection(board, Position, -1, -1)); // lower-left
            return validMoves;
        }
    }
}