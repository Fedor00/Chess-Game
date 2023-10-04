using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Logic;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class ChessGameRepository : IChessGameRepository
    {
        private readonly DataContext _context;

        public ChessGameRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<ChessGame> GetWaitingGame()
        {
            return await _context.ChessGames.Include(g => g.BottomPlayer).Include(g => g.TopPlayer).FirstOrDefaultAsync(g => g.Status == GameStatus.Waiting);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task AddAsync(ChessGame game)
        {
            await _context.ChessGames.AddAsync(game);

        }
        public void UpdateAsync(ChessGame game)
        {
            _context.Entry(game).State = EntityState.Modified;

        }

        public async Task<ChessGame> FindAsync(Guid id)
        {
            return await _context.ChessGames
                        .Include(g => g.BottomPlayer)
                        .Include(g => g.TopPlayer)
                        .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<ChessGame> GetCurrentGameForPlayer(int userId)
        {
            return await _context.ChessGames
                         .Include(g => g.BottomPlayer)
                         .Include(g => g.TopPlayer)
                         .FirstOrDefaultAsync(g =>
                             (g.TopPlayerId == userId || g.BottomPlayerId == userId) &&
                             g.Status == GameStatus.Waiting || g.Status == GameStatus.InProgress
                         );
        }

    }
}