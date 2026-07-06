using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Infrastructure;

[Authorize]
public class ChatHub : Hub
{
    private readonly IAIService _ai;

    public ChatHub(IAIService ai)
    {
        _ai = ai;
    }

    public async Task SendMessage(string message)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return;

        await Clients.All.SendAsync("ReceiveMessage", "You", message);

        var aiResponse = await _ai.GetResponseAsync(Guid.Parse(userId), message);

        await Clients.All.SendAsync("ReceiveMessage", "AI Bot", aiResponse);
    }
}