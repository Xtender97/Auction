using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class AuctionHub : Hub
    {
        public async Task NotifyAll(string auctionId)
        {
            await Clients.All.SendAsync("Update", auctionId);
        }
    }
}