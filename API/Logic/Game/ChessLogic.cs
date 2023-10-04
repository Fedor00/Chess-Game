using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using API.Entities;
namespace API.Logic
{
    public class ChessLogic
    {
        public const string InitialFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        public ChessLogic(string currentFEN)
        {
            this.CurrentFEN = currentFEN;
            this.ChessBoard = new(currentFEN);
        }

        public ChessBoard ChessBoard { get; set; }
        public User TopPlayer { get; set; }
        public User BottomPlayer { get; set; }
        public User CurrentPlayer { get; set; }
        public string CurrentFEN { get; set; }
        public GameStatus Status { get; set; }
        public ChessLogic()
        {
            this.CurrentFEN = InitialFEN;
            this.ChessBoard = new(InitialFEN);
        }
        public ChessLogic(User topPlayer, User bottomPlayer, string currentFEN, GameStatus status)
        {
            this.CurrentFEN = currentFEN;
            this.ChessBoard = new(currentFEN);
            this.Status = status;
            TopPlayer = topPlayer;
            BottomPlayer = bottomPlayer;
            char activeColor = currentFEN.Split(' ')[1][0];
            if (activeColor == 'w')
            {
                this.CurrentPlayer = BottomPlayer;
            }
            else if (activeColor == 'b')
            {
                this.CurrentPlayer = TopPlayer;
            }
        }

        public List<Move> MovesHistory { get; } = new List<Move>();
        public bool MakeMove(Move move, User player)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            if (!player.Equals(CurrentPlayer)) return false;
            Color currentColor = GetCurrentColor();
            if (currentColor.Equals(Color.Black))
            {
                move.From.Rank = 7 - move.From.Rank;
                move.From.File = 7 - move.From.File;
                move.To.Rank = 7 - move.To.Rank;
                move.To.File = 7 - move.To.File;
            }
            Piece PieceToMove = ChessBoard.GetPieceAt(move.From);
            if (PieceToMove == null) return false;

            Piece PieceToCapture = ChessBoard.GetPieceAt(move.To);

            if (currentColor != PieceToMove.PieceColor) return false;

            if (PieceToCapture != null && PieceToCapture.PieceColor.Equals(PieceToMove.PieceColor)) return false;

            if (IsEnPassantMove(move))
            {
                if (!IsValidEnPassant(move)) return false;
                ChessBoard.ApplyEnPassant(move);

            }
            else if (IsCastlingMove(move))
            {
                if (IsValidCastling())
                {
                    ChessBoard.ApplyCastling(move);
                }
                else
                    return false;
            }
            else
            {
                if (!IsValidMove(move)) return false;
                ChessBoard.ApplyMove(move);
            }

            if (PieceToMove.Symbol == 'P')
            {
                if (move.To.Rank == 0 || move.To.Rank == 7)
                {
                    Piece piece = ChessBoard.GetPieceAt(move.To);

                    var q = char.IsUpper(piece.Symbol) ? 'Q' : 'q';
                    ChessBoard[move.To.Rank, move.To.File] = ChessBoard.CreatePieceFromChar(q, move.To.Rank, move.To.File);
                }
            }
            // Switch current player
            SwitchCurrentPlayer();
            if (IsCheckmate(GetCurrentColor()))
            {
                this.Status = GameStatus.Checkmate;
            }

            UpdateFen(move);

            stopwatch.Stop();

            Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} ms");
            return true;
        }


        private bool IsCheckmate(Color currentColor)
        {
            if (!ChessBoard.IsKingInCheck(currentColor))
            {
                return false; // If king is not in check, it can't be checkmate
            }


            // Iterate over the board using nested for loops
            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {
                    Piece currentPiece = ChessBoard[rank, file];

                    if (currentPiece != null && currentPiece.PieceColor == currentColor)
                    {
                        Position currentPiecePosition = new Position(rank, file);
                        List<Position> validMovesForPiece = currentPiece.ValidMoves(ChessBoard);

                        // For each valid move, check if it leaves the king in check
                        foreach (Position targetPosition in validMovesForPiece)
                        {
                            Move potentialMove = new Move(currentPiecePosition, targetPosition);
                            if (!LeavesOrPutsKingInCheck(potentialMove))
                            {
                                return false;  // There's at least one move that doesn't leave the king in check
                            }
                        }
                    }
                }
            }



            // If we reach here, every move for every piece leaves the king in check, so it's checkmate
            return true;
        }

        private void UpdateFen(Move move)
        {

            string boardState = "";
            for (int rank = 0; rank < 8; rank++) // starting from the top
            {
                int emptySquares = 0;
                for (int file = 0; file < 8; file++)
                {
                    Piece piece = ChessBoard[rank, file];
                    if (piece == null)
                    {
                        emptySquares++;
                    }
                    else
                    {
                        if (emptySquares > 0)
                        {
                            boardState += emptySquares;
                            emptySquares = 0;
                        }
                        boardState += piece.ToFenChar();
                    }
                }
                if (emptySquares > 0)
                    boardState += emptySquares;

                if (rank < 7)
                    boardState += "/";
            }

            // 2. Update the active color.
            string activeColor = GetCurrentColor() == Color.White ? "w" : "b";

            // 3. Update the castling rights.
            string castlingRights = CurrentFEN.Split(' ')[2];
            //update enPassant
            string enPassant = "-";
            if (ChessBoard.GetPieceAt(move.To).Symbol == 'P' && Math.Abs(move.From.Rank - move.To.Rank) == 2)
            {
                int enPassantRank = (move.From.Rank + move.To.Rank) / 2;
                int enPassantFile = move.To.File; // the file remains the same for en passant
                enPassant = ChessBoard.ConvertToSquareNotation(new Position(enPassantRank, enPassantFile));
            }
            // Increase it after every move. Reset to 0 after a pawn move or capture.
            int halfmoveClock = int.Parse(CurrentFEN.Split(' ')[4]); // You might want to store this separately for efficiency.
            halfmoveClock++; // placeholder, adjust according to the move type

            // 6. Update the fullmove number.
            int fullmoveNumber = int.Parse(CurrentFEN.Split(' ')[5]);
            if (GetCurrentColor() == Color.Black) // Increase the move count after black's move.
                fullmoveNumber++;

            CurrentFEN = $"{boardState} {activeColor} {castlingRights} {enPassant} {halfmoveClock} {fullmoveNumber}";

        }
        public void Resign(User player) { }
        private bool IsValidMove(Move move)
        {
            Piece pieceToCapture = ChessBoard.GetPieceAt(move.From);
            if (LeavesOrPutsKingInCheck(move)) return false;

            List<Position> validMoves = pieceToCapture.ValidMoves(ChessBoard);
            if (!validMoves.Contains(move.To)) return false;

            return true;
        }
        private bool FenAllowsKingsideCastling(Color color)
        {
            string castlingRights = CurrentFEN.Split(' ')[2];

            if (color == Color.White)
            {
                return castlingRights.Contains('K');
            }
            else // assuming Color.Black
            {
                return castlingRights.Contains('k');
            }
        }
        private bool FenAllowsQueensideCastling(Color color)
        {
            string castlingRights = CurrentFEN.Split(' ')[2];

            if (color == Color.White)
            {
                return castlingRights.Contains('Q');
            }
            else // assuming Color.Black
            {
                return castlingRights.Contains('q');
            }
        }
        private bool IsValidCastling()
        {
            Color currentColor = GetCurrentColor();

            if (ChessBoard.IsKingInCheck(currentColor)) return false;

            // Assuming `GetPotentialCaptures` returns a list of positions under attack for a given color.
            List<Position> squaresUnderAttack = ChessBoard.ValidMovesForColor(GetOpponentColor());

            // For kingside castling
            if (FenAllowsKingsideCastling(currentColor))
            {
                Position kingsideSquare1 = currentColor == Color.White ? new Position(0, 5) : new Position(7, 5); // f or f8
                Position kingsideSquare2 = currentColor == Color.White ? new Position(0, 6) : new Position(7, 6); // g or g8
                // Check if squares are empty and not attacked
                if (ChessBoard.GetPieceAt(kingsideSquare1) == null &&
                    ChessBoard.GetPieceAt(kingsideSquare2) == null &&
                    !squaresUnderAttack.Contains(kingsideSquare1) &&
                    !squaresUnderAttack.Contains(kingsideSquare2))
                {
                    return true;
                }
            }

            // For queenside castling
            if (FenAllowsQueensideCastling(currentColor))
            {
                Position queensideSquare1 = currentColor == Color.White ? new Position(0, 3) : new Position(7, 3); // d or d8
                Position queensideSquare2 = currentColor == Color.White ? new Position(0, 2) : new Position(7, 2); // c or c8
                // Check if squares are empty and not attacked
                if (ChessBoard.GetPieceAt(queensideSquare1) == null &&
                    ChessBoard.GetPieceAt(queensideSquare2) == null &&
                    !squaresUnderAttack.Contains(queensideSquare1) &&
                    !squaresUnderAttack.Contains(queensideSquare2))
                {
                    return true;
                }
            }

            return false;
        }
        private bool IsCastlingMove(Move move)
        {
            Piece movedPiece = ChessBoard.GetPieceAt(move.From);

            if (movedPiece == null || movedPiece.Symbol != 'K') return false;

            int moveDistance = move.To.File - move.From.File;

            return moveDistance switch
            {
                // King-side castling
                2 => ChessBoard.GetPieceAt(new Position(move.From.Rank, 7))?.Symbol == 'R',
                // Queen-side castling
                -2 => ChessBoard.GetPieceAt(new Position(move.From.Rank, 0))?.Symbol == 'R',
                _ => false,
            };
        }
        private bool IsEnPassantMove(Move move)
        {
            string enPassant = CurrentFEN.Split(' ')[3];
            if (enPassant.Equals("-")) return false;
            if (ChessBoard.GetPieceAt(move.From).Symbol != 'P') return false;
            Position fenEnPassantPosition = ChessBoard.ConvertToBoardCoordinates(enPassant);
            if (!move.To.Equals(fenEnPassantPosition)) return false;
            return true;
        }
        private bool IsValidEnPassant(Move move)
        {
            if (LeavesOrPutsKingInCheck(move)) return false;
            return true;
        }
        private bool LeavesOrPutsKingInCheck(Move move)
        {
            Color currentColor = GetCurrentColor();
            Color opponentColor = GetOpponentColor();
            // Backup original state
            Piece originalFromPiece = ChessBoard.GetPieceAt(move.From);
            Piece originalToPiece = ChessBoard.GetPieceAt(move.To);
            // Apply the move
            ChessBoard[move.To.Rank, move.To.File] = ChessBoard[move.From.Rank, move.From.File];
            ChessBoard[move.From.Rank, move.From.File] = null;
            // Check for threats against the king
            Position ourKingPosition = ChessBoard.FindKingPosition(currentColor);
            bool kingIsThreatened = ChessBoard.GetPotentialCaptures(opponentColor).Contains(ourKingPosition);
            // Revert the move
            ChessBoard[move.From.Rank, move.From.File] = originalFromPiece;
            ChessBoard[move.To.Rank, move.To.File] = originalToPiece;
            return kingIsThreatened;
        }
        private void SwitchCurrentPlayer()
        {
            CurrentPlayer = CurrentPlayer.Equals(TopPlayer) ? BottomPlayer : TopPlayer;
        }
        private Color GetCurrentColor()
        {
            return CurrentPlayer.Equals(TopPlayer) ? Color.Black : Color.White;
        }
        private Color GetOpponentColor()
        {
            return CurrentPlayer.Equals(TopPlayer) ? Color.White : Color.Black;
        }

    }
}

