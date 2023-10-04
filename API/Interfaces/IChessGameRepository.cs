using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IChessGameRepository
    {
        Task<ChessGame> GetWaitingGame();
        Task<ChessGame> GetCurrentGameForPlayer(int userId);
        Task<ChessGame> FindAsync(Guid id);
        Task AddAsync(ChessGame game);
        void UpdateAsync(ChessGame game);
        Task<bool> SaveChangesAsync();
    }



}