
using API.Logic;

namespace API.Entities
{
    public class ChessGame
    {
        public Guid Id { get; set; }
        public int? TopPlayerId { get; set; }
        public int? BottomPlayerId { get; set; }

        public User TopPlayer { get; set; }
        public User BottomPlayer { get; set; }
        public GameStatus Status { get; set; }

        public string CurrentFEN { get; set; }


    }
}