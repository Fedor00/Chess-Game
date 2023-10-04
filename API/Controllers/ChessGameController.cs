using Microsoft.AspNetCore.Mvc;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using API.DTOs;
using System.Security.Claims;
using API.Exceptions;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers
{
    [Authorize]
    public class ChessGameController : BaseApiController
    {
        private readonly IChessGameService _service;


        public ChessGameController(IChessGameService service)
        {
            _service = service;
        }

        [HttpPost("random")]
        public async Task<IActionResult> CreateOrJoinRandomGame()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var gameDto = await _service.CreateOrJoinRandomGame(userId);
                return Ok(gameDto);
            }
            catch (GameNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (AlreadyPlayingException ex)
            {
                Console.WriteLine(ex.Message);
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Ideally, log the exception here
                // e.g., _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }


        [HttpPost("move")]
        [HttpPost]
        public async Task<IActionResult> MakeMove(MoveDto moveDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var gameDto = await _service.MakeMove(moveDto, userId);
                return Ok(gameDto);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (GameNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UserNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidMoveException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (DataUpdateException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (SignalRException ex)
            {
                return StatusCode(500, new { message = "Server error: " + ex.Message });
            }
            catch (Exception ex)
            {
                // Ideally, log the exception here
                // e.g., _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }


        [HttpGet("current-game")]
        public async Task<IActionResult> GetCurrentGame()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var game = await _service.GetCurrentGameForPlayer(userId);

                if (game == null)
                    return NotFound("No ongoing game found for the player.");

                return Ok(game);
            }
            catch (GameNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal error occurred. Please try again later.");
            }
        }
    }

}
