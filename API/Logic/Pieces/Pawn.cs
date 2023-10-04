using System;
using System.Collections.Generic;
using System.Linq;


namespace API.Logic
{
    public class Pawn : Piece
    {
        public Pawn(Color color, Position position) : base(color, position)
        {
        }

        public override char Symbol => 'P';

        public override List<Position> PotentialCaptures(ChessBoard board)
        {
            return ValidMoves(board).Where(move => board[move.Rank, move.File] != null).ToList();
        }

        public override List<Position> ValidMoves(ChessBoard board)
        {
            List<Position> moves = new();
            int forward = (PieceColor == Color.White) ? -1 : 1;

            // Regular Move
            Position forwardPos = new(Position.Rank + forward, Position.File);
            if (IsPositionValid(forwardPos) && board[forwardPos.Rank, forwardPos.File] == null)
            {
                moves.Add(forwardPos);

                // Double Move at the start
                if ((PieceColor == Color.Black && Position.Rank == 1) ||
                    (PieceColor == Color.White && Position.Rank == 6))
                {
                    Position doubleForwardPos = new(Position.Rank + 2 * forward, Position.File);
                    if (IsPositionValid(doubleForwardPos) && board[doubleForwardPos.Rank, doubleForwardPos.File] == null)
                    {
                        moves.Add(doubleForwardPos);
                    }
                }
            }

            // Captures
            for (int i = -1; i <= 1; i += 2)
            {
                Position capturePos = new(Position.Rank + forward, Position.File + i);
                if (IsPositionValid(capturePos))
                {
                    Piece pieceAtCapturePos = board[capturePos.Rank, capturePos.File];
                    if (pieceAtCapturePos != null && pieceAtCapturePos.PieceColor != this.PieceColor)
                    {
                        moves.Add(capturePos);
                    }
                }
            }

            return moves;
        }


    }
}