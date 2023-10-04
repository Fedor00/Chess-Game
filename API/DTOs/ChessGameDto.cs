using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Logic;

namespace API.DTOs
{
    public class ChessGameDto
    {
        public Guid Id { get; set; }
        public Piece[,] Board { get; set; }
        public string TopPlayerName { get; set; }
        public string BottomPlayerName { get; set; }
        public string TopPlayerEmail { get; set; }
        public string BottomPlayerEmail { get; set; }
        public GameStatus Status { get; set; }
    }
}