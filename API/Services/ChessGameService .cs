using System;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Exceptions;
using API.Interfaces;
using API.Logic;
using Microsoft.AspNetCore.SignalR;

namespace API.Services
{
    public class ChessGameService : IChessGameService
    {
        private const string MoveMadeSignalRMethod = "MoveMade";
        private const string GameCreatedOrJoinedSignalRMethod = "RefreshGame";

        private readonly IChessGameRepository _chessGameRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<ChessGameHub> _hubContext;

        public ChessGameService(IChessGameRepository chessGameRepository, IUserRepository userRepository, IHubContext<ChessGameHub> hubContext)
        {
            _chessGameRepository = chessGameRepository;
            _userRepository = userRepository;
            _hubContext = hubContext;
        }

        public async Task<ChessGameDto> CreateOrJoinRandomGame(int userId)
        {
            var alreadyPlaying = await _chessGameRepository.GetCurrentGameForPlayer(userId);
            if (alreadyPlaying != null) throw new AlreadyPlayingException("Already started a game.");
            var waitingGame = await _chessGameRepository.GetWaitingGame();
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
                throw new UserNotFoundException("User not found");

            if (waitingGame != null)
            {
                await AssignUserToWaitingGameAsync(waitingGame, user);
            }
            else
            {
                return await CreateNewGameForUser(user);
            }

            return ConvertToDto(waitingGame);
        }

        public async Task<ChessGameDto> MakeMove(MoveDto moveDto, int userId)
        {
            if (moveDto == null)
                throw new ArgumentNullException("Invalid move.");

            var game = await _chessGameRepository.FindAsync(Guid.Parse(moveDto.gameId));
            if (game == null)
                throw new GameNotFoundException("Game not found");

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new UserNotFoundException("User not found");

            ChessLogic chessLogic = new ChessLogic(game.TopPlayer, game.BottomPlayer, game.CurrentFEN, game.Status);
            bool moved = chessLogic.MakeMove(new Move(moveDto.From, moveDto.To), user);

            if (!moved)
                throw new InvalidMoveException("Invalid move attempted.");

            game.CurrentFEN = chessLogic.CurrentFEN;
            game.Status = chessLogic.Status;
            _chessGameRepository.UpdateAsync(game);
            if (!await _chessGameRepository.SaveChangesAsync())
            {
                throw new DataUpdateException("Error saving game move.");
            }
            var gameDto = ConvertToDtoFromLogic(chessLogic, game);
            try
            {
                await _hubContext.Clients.Group(gameDto.Id.ToString()).SendAsync(MoveMadeSignalRMethod, gameDto);
            }
            catch (Exception ex)
            {
                throw new SignalRException("Failed to send move to clients");
            }

            return gameDto;
        }

        public async Task<ChessGameDto> GetCurrentGameForPlayer(int userId)
        {
            var game = await _chessGameRepository.GetCurrentGameForPlayer(userId);
            if (game == null)
                throw new GameNotFoundException("No current game for the specified user.");

            return ConvertToDto(game);
        }

        public async Task<ChessGameDto> GetGameById(Guid id)
        {
            var game = await _chessGameRepository.FindAsync(id);
            if (game == null)
                throw new GameNotFoundException("Game with the specified ID not found.");

            return ConvertToDto(game);
        }
        private async Task AssignUserToWaitingGameAsync(ChessGame waitingGame, User user)
        {
            waitingGame.TopPlayer = user;
            waitingGame.TopPlayerId = user.Id;
            waitingGame.Status = GameStatus.InProgress;

            _chessGameRepository.UpdateAsync(waitingGame);
            if (!await _chessGameRepository.SaveChangesAsync())
            {
                throw new DataUpdateException("Error updating the waiting game.");
            }

            try
            {

                await _hubContext.Clients.Group(waitingGame.Id.ToString())
                    .SendAsync(GameCreatedOrJoinedSignalRMethod, ConvertToDto(waitingGame)); // Assuming RefreshGameSignalRMethod is a constant string representing the event name
            }
            catch (Exception ex)
            {
                throw new SignalRException("Failed to notify clients about updated game", ex);
            }
        }


        private async Task<ChessGameDto> CreateNewGameForUser(User user)
        {
            ChessGame chessGame = new ChessGame
            {
                Id = Guid.NewGuid(),
                BottomPlayer = user,
                BottomPlayerId = user.Id,
                Status = GameStatus.Waiting,
                CurrentFEN = ChessLogic.InitialFEN
            };

            await _chessGameRepository.AddAsync(chessGame);
            if (!await _chessGameRepository.SaveChangesAsync())
            {
                throw new DataUpdateException("Error creating a new game.");
            }
            Console.WriteLine("NOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
            try
            {
                Console.WriteLine("I was here.");
                await _hubContext.Clients.Group(chessGame.Id.ToString()).SendAsync(GameCreatedOrJoinedSignalRMethod, ConvertToDto(chessGame));

            }
            catch (Exception ex)
            {
                throw new SignalRException("Failed to notify clients about new game", ex);
            }

            return ConvertToDto(chessGame);
        }

        private ChessGameDto ConvertToDto(ChessGame game)
        {
            ChessLogic chessLogic = new ChessLogic(game.TopPlayer, game.BottomPlayer, game.CurrentFEN, game.Status);
            return new ChessGameDto
            {
                Board = chessLogic.ChessBoard.Board,
                Status = chessLogic.Status,
                TopPlayerName = game.TopPlayer?.Username,
                BottomPlayerName = game.BottomPlayer?.Username,
                TopPlayerEmail = game.TopPlayer?.Email,
                BottomPlayerEmail = game.BottomPlayer?.Email,
                Id = game.Id
            };
        }
        private ChessGameDto ConvertToDtoFromLogic(ChessLogic chessLogic, ChessGame game)
        {
            return new ChessGameDto
            {
                Board = chessLogic.ChessBoard.Board,
                Status = chessLogic.Status,
                TopPlayerName = chessLogic.TopPlayer?.Username,
                BottomPlayerName = chessLogic.BottomPlayer?.Username,
                TopPlayerEmail = game.TopPlayer?.Email,
                BottomPlayerEmail = game.BottomPlayer?.Email,
                Id = game.Id
            };
        }
    }
}
