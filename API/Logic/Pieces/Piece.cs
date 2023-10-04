
namespace API.Logic
{
    public abstract class Piece
    {
        public Position Position { get; set; }
        public Color PieceColor { get; set; }
        public abstract char Symbol { get; }
        public abstract List<Position> ValidMoves(ChessBoard board);
        public abstract List<Position> PotentialCaptures(ChessBoard board);
        protected Piece(Color PieceColor, Position Position)
        {
            this.PieceColor = PieceColor;
            this.Position = Position;
        }
        public char ToFenChar()
        {
            if (PieceColor == Color.White)
                return Symbol;
            else
                return char.ToLower(Symbol);
        }
        protected List<Position> GetMovesInDirection(ChessBoard board, Position startPos, int dirX, int dirY)
        {
            List<Position> positions = new();
            Position newPos = new(startPos.Rank + dirX, startPos.File + dirY);

            while (newPos.Rank >= 0 && newPos.Rank < 8 && newPos.File >= 0 && newPos.File < 8)
            {
                Piece occupyingPiece = board[newPos.Rank, newPos.File];

                if (occupyingPiece == null)
                {
                    // If square is empty, we can move there
                    positions.Add(newPos);
                    newPos = new Position(newPos.Rank + dirX, newPos.File + dirY);
                }
                else
                {
                    // If square is occupied by an opposite color piece, it's a valid capture move
                    if (occupyingPiece.PieceColor != this.PieceColor)
                    {
                        positions.Add(newPos);
                    }
                    break; // stop in both cases (either same color or opposite color piece)
                }
            }
            return positions;
        }
        public bool IsPositionValid(Position pos)
        {
            return pos.Rank >= 0 && pos.Rank < 8 && pos.File >= 0 && pos.File < 8;
        }
    }

}