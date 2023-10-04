using System.Security.Claims;
using API.Interfaces;
using Microsoft.AspNetCore.SignalR;

public class ChessGameHub : Hub
{
    private readonly IChessGameService _service;

    public ChessGameHub(IChessGameService service)
    {
        _service = service;
    }

    public async Task JoinGameGroup(string gameId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        await Clients.Caller.SendAsync("JoinedGame", gameId);
    }

    // ... rest of your hub methods
}
