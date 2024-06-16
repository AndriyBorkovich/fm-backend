using Microsoft.AspNetCore.SignalR;

namespace FootballManager.API.Hubs;

/// <summary>
/// Counts current active users
/// </summary>
public class ViewCountHub : Hub
{
    public static int ViewCount { get; set; } = 0;

    public async Task NotifyWatching()
    {
        ViewCount++;

        // notify everyone
        await Clients.All.SendAsync("viewCountUpdate", ViewCount);
    }

    public async Task CheckCurrent()
    {
        await Clients.All.SendAsync("viewCountUpdate", ViewCount);
    }

    public async Task NotifyUnwatching()
    {
        ViewCount--;

        await Clients.All.SendAsync("viewCountUpdate", ViewCount);
    }
}
