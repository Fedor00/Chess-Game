using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Exceptions
{
    public class GameNotFoundException : Exception
    {
        public GameNotFoundException(string gameId) : base($"No game found with ID {gameId}")
        {
        }
        public GameNotFoundException(int userId) : base($"No open game found for USER_ID {userId}")
        {
        }
    }

}