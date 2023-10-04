
namespace API.Logic
{
    public class Queen : Piece
    {
        public Queen(Color color, Position position) : base(color, position)
        {
        }

        public override char Symbol => 'Q';

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
            validMoves.AddRange(GetMovesInDirection(board, Position, 0, 1));  // right
            validMoves.AddRange(GetMovesInDirection(board, Position, 0, -1)); // left
            validMoves.AddRange(GetMovesInDirection(board, Position, 1, 0));  // up
            validMoves.AddRange(GetMovesInDirection(board, Position, -1, 0)); // down
            return validMoves;
        }
    }
}