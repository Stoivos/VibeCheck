using Microsoft.AspNetCore.SignalR;
namespace VibeCheck.Server.Hubs
{
    public class CrowdHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("Connected", "Connected to CrowdHub");
            await base.OnConnectedAsync();
        }
    }
}
