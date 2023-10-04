using System;


namespace API.Logic
{
    public class ChessBoard
    {
        private const int BOARD_SIZE = 8;
        public Piece[,] Board { get; }

        public ChessBoard(string InitialFEN)
        {
            Board = new Piece[BOARD_SIZE, BOARD_SIZE];
            InitializeFromFEN(InitialFEN);
        }

        public Piece this[int rank, int file]
        {
            get => Board[rank, file];
            set => Board[rank, file] = value;
        }

        private void InitializeFromFEN(string fen)
        {
            string[] parts = fen.Split(' ');
            if (parts.Length < 1) throw new ArgumentException("Invalid FEN string.");

            SetPiecesOnBoard(parts[0]);
        }

        public void ReverseMoveCoordinates(Move move)
        {
            move.From = new Position(BOARD_SIZE - move.From.Rank - 1, BOARD_SIZE - move.From.File - 1);
            move.To = new Position(BOARD_SIZE - move.To.Rank - 1, BOARD_SIZE - move.To.File - 1);
        }
        private void SetPiecesOnBoard(string piecePlacement)
        {
            int rank = 0;  // Start from the top rank (8th rank in traditional chess)
            int file = 0;  // Start from the leftmost file (a-file in traditional chess)

            foreach (char ch in piecePlacement)
            {
                switch (ch)
                {
                    case '/':
                        rank++;
                        file = 0;
                        break;
                    case char digit when char.IsDigit(digit):
                        file += int.Parse(digit.ToString());
                        break;
                    default:
                        Board[rank, file] = CreatePieceFromChar(ch, rank, file);
                        file++;
                        break;
                }
            }
        }

        public void ApplyMove(Move Move)
        {
            //Upon moving, the piece will be assigned to a new tile and, consequently, a new position.
            Board[Move.From.Rank, Move.From.File].Position = Move.To;
            //Move Piece to new Tile
            Board[Move.To.Rank, Move.To.File] = Board[Move.From.Rank, Move.From.File];
            Board[Move.From.Rank, Move.From.File] = null;
        }
        public void ApplyEnPassant(Move move)
        {
            // Apply the pawn's move first
            ApplyMove(move);

            // Determine the opponent's pawn position that was bypassed
            int capturedPawnRank;
            if (Board[move.To.Rank, move.To.File].PieceColor == Color.White)
            {
                // If the moving pawn is white, the captured pawn will be one rank below
                capturedPawnRank = move.To.Rank - 1;
            }
            else
            {
                // If the moving pawn is black, the captured pawn will be one rank above
                capturedPawnRank = move.To.Rank + 1;
            }

            // Remove the opponent's pawn
            Board[capturedPawnRank, move.To.File] = null;
        }

        public void RevertMove(Move Move, Piece capturedPiece)
        {
            // Move the piece back to its original position
            Board[Move.From.Rank, Move.From.File] = Board[Move.To.Rank, Move.To.File];
            Board[Move.From.Rank, Move.From.File].Position = Move.From;

            // Restore the captured piece (if any) to its original position
            Board[Move.To.Rank, Move.To.File] = capturedPiece;
        }

        public static Piece CreatePieceFromChar(char ch, int rank, int file)
        {
            Color color = char.IsUpper(ch) ? Color.White : Color.Black;
            char pieceType = char.ToUpper(ch);

            Position position = new(rank, file);

            return pieceType switch
            {
                'P' => new Pawn(color, position),
                'R' => new Rook(color, position),
                'N' => new Knight(color, position),
                'B' => new Bishop(color, position),
                'Q' => new Queen(color, position),
                'K' => new King(color, position),
                _ => throw new ArgumentException($"Invalid character: {ch}")
            };
        }

        public Piece GetPieceAt(Position Position)
        {
            return Board[Position.Rank, Position.File];
        }
        public List<Piece> GetPiecesOfColor(Color color)
        {
            List<Piece> piecesOfColor = new();

            for (int rank = 0; rank < BOARD_SIZE; rank++)
            {
                for (int file = 0; file < BOARD_SIZE; file++)
                {
                    Piece piece = Board[rank, file];
                    if (piece != null && piece.PieceColor == color)
                    {
                        piecesOfColor.Add(piece);
                    }
                }
            }

            return piecesOfColor;
        }
        public List<Position> ValidMovesForColor(Color color)
        {
            List<Position> validMoves = new();
            foreach (Piece piece in Board)
            {
                if (piece != null && piece.PieceColor == color)
                    validMoves.AddRange(piece.ValidMoves(this));
            }
            return validMoves;
        }
        public List<Position> GetPotentialCaptures(Color color)
        {
            List<Position> validMoves = new();

            foreach (Piece piece in Board)
            {
                if (piece != null && piece.PieceColor == color)
                {
                    var potentialCaptures = piece.PotentialCaptures(this);

                    validMoves.AddRange(potentialCaptures);
                }
            }
            return validMoves;
        }
        public List<Piece> GetPinnedPieces(Color color)
        {
            List<Piece> pinnedPieces = new();
            Position kingPosition = FindKingPosition(color);
            // The directions for rook and queen
            List<Position> rankFileDirections = new()
            {
                new Position(1, 0),  // Up
                new Position(-1, 0), // Down
                new Position(0, 1),  // Right
                new Position(0, -1)  // Left
            };
            // The directions for bishop and queen
            List<Position> diagonalDirections = new()
            {
                new Position(1, 1),   // Up-Right
                new Position(1, -1),  // Up-Left
                new Position(-1, 1),  // Down-Right
                new Position(-1, -1)  // Down-Left
            };
            foreach (var direction in rankFileDirections.Concat(diagonalDirections))
            {
                Piece potentialPinnedPiece = null;
                for (int distance = 1; distance < 8; distance++)
                {
                    int rankOffset = direction.Rank * distance;
                    int fileOffset = direction.File * distance;
                    Position currentPos = new(kingPosition.Rank + rankOffset, kingPosition.File + fileOffset);
                    // If out of bounds, break
                    if (!IsValidPosition(currentPos))
                        break;
                    Piece currentPiece = GetPieceAt(currentPos);
                    // If it's an empty square, continue
                    if (currentPiece == null)
                        continue;
                    // If it's a friendly piece and we haven't found a potential pinned piece yet
                    if (currentPiece.PieceColor == color && potentialPinnedPiece == null)
                    {
                        potentialPinnedPiece = currentPiece;
                        continue;
                    }
                    // If it's an enemy piece and we found a potential pinned piece
                    if (currentPiece.PieceColor != color && potentialPinnedPiece != null)
                    {
                        // Check for rook/queen on rank/file or bishop/queen on diagonal
                        bool isRookOrQueen = currentPiece.Symbol == 'R' || currentPiece.Symbol == 'Q';
                        bool isBishopOrQueen = currentPiece.Symbol == 'B' || currentPiece.Symbol == 'Q';

                        if ((rankFileDirections.Contains(direction) && isRookOrQueen) ||
                            (diagonalDirections.Contains(direction) && isBishopOrQueen))
                        {
                            pinnedPieces.Add(potentialPinnedPiece);
                        }
                        break; // Break out of the for loop since we found an enemy piece
                    }
                    // If it's another enemy piece and no pinned piece was found, break
                    if (currentPiece.PieceColor != color)
                        break;
                }
            }
            return pinnedPieces;
        }

        private List<Position> FilterPinnedPieceMoves(List<Position> moves, Piece pinnedPiece)
        {
            Position kingPosition = FindKingPosition(pinnedPiece.PieceColor);
            Position pinningPiecePosition = pinnedPiece.Position;
            var positionsToBlock = PositionsToBlockThreat(kingPosition, pinningPiecePosition);
            return moves.Where(move => positionsToBlock.Contains(move)).ToList();
        }
        public void ApplyCastling(Move move)
        {
            Position kingOriginalPos = move.From;
            Position kingNewPos = move.To;
            Position rookOriginalPos;
            Position rookNewPos;

            // Determine if it's kingside or queenside castling based on the king's move
            if (kingNewPos.File - kingOriginalPos.File > 0)  // Kingside
            {
                rookOriginalPos = new Position(kingOriginalPos.Rank, 7); // h or h8
                rookNewPos = new Position(kingOriginalPos.Rank, 5);     // f or f8
            }
            else  // Queenside
            {
                rookOriginalPos = new Position(kingOriginalPos.Rank, 0); // a or a8
                rookNewPos = new Position(kingOriginalPos.Rank, 3);     // d or d8
            }

            // Move the king and update its position
            Piece king = Board[kingOriginalPos.Rank, kingOriginalPos.File];
            king.Position = kingNewPos;
            Board[kingNewPos.Rank, kingNewPos.File] = king;
            Board[kingOriginalPos.Rank, kingOriginalPos.File] = null;

            // Move the rook and update its position
            Piece rook = Board[rookOriginalPos.Rank, rookOriginalPos.File];
            rook.Position = rookNewPos;
            Board[rookNewPos.Rank, rookNewPos.File] = rook;
            Board[rookOriginalPos.Rank, rookOriginalPos.File] = null;

            // If you're updating the FEN or another internal state to reflect that castling has occurred, you can do it here.
        }


        public bool IsKingInCheck(Color color)
        {
            Color opponentColor = color.Equals(Color.Black) ? Color.White : Color.Black;
            Position ourKingPosition = FindKingPosition(color);
            return GetPotentialCaptures(opponentColor).Contains(ourKingPosition);
        }
        public Position FindKingPosition(Color playerColor)
        {
            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {
                    Piece piece = GetPieceAt(new Position(rank, file));
                    if (piece is King && piece.PieceColor == playerColor)
                    {
                        return new Position(rank, file);
                    }
                }
            }
            throw new Exception("King not found!"); // This should never happen
        }
        public Position ConvertToBoardCoordinates(string square)
        {
            int x = square[0] - 'a'; // Convert file to x-coordinate
            int y = 8 - (square[1] - '0'); // Convert rank to y-coordinate

            return new Position(x, y);
        }
        public string ConvertToSquareNotation(Position position)
        {
            char file = (char)('a' + position.Rank);
            int rank = 8 - position.File;
            return $"{file}{rank}";
        }

        public List<Position> IdentifyThreatsToKing(Position kingPosition, Color kingColor)
        {
            List<Position> threats = new();
            Color opponentColor = kingColor == Color.White ? Color.Black : Color.White;
            foreach (Position pos in GetPotentialCaptures(opponentColor))
            {
                if (pos.Equals(kingPosition))
                {
                    threats.Add(pos);
                }
            }
            return threats;
        }

        private List<Position> PositionsToBlockThreat(Position kingPosition, Position threatPosition)
        {
            // Implement a way to determine the positions between king and the threat that can be occupied to block the threat.
            List<Position> blockPositions = new();

            int rankIncrement = (threatPosition.Rank > kingPosition.Rank) ? 1 : (threatPosition.Rank < kingPosition.Rank) ? -1 : 0;
            int fileIncrement = (threatPosition.File > kingPosition.File) ? 1 : (threatPosition.File < kingPosition.File) ? -1 : 0;

            Position currentPosition = new(kingPosition.Rank + rankIncrement, kingPosition.File + fileIncrement);
            while (!currentPosition.Equals(threatPosition))
            {
                blockPositions.Add(currentPosition);
                currentPosition = new Position(currentPosition.Rank + rankIncrement, currentPosition.File + fileIncrement);
            }
            return blockPositions;
        }
        public bool IsValidPosition(Position pos)
        {
            return pos.Rank >= 0 && pos.Rank < 8 && pos.File >= 0 && pos.File < 8;
        }


    }



}

