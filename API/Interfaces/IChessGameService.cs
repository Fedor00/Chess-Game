using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IChessGameService
    {
        Task<ChessGameDto> CreateOrJoinRandomGame(int userId);
        Task<ChessGameDto> MakeMove(MoveDto moveDto, int userId);
        Task<ChessGameDto> GetCurrentGameForPlayer(int userId);
        Task<ChessGameDto> GetGameById(Guid id);

    }

}