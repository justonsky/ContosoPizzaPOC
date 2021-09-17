// unset

using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ContosoPizza.Hubs
{
    public class CustomerHub : Hub
    {
        // The server code in a Hub defines methods that are called by the CLIENT.
        public async Task SendCustomerSupportMessage(string user, object message)
        {
            await Clients.User(user).SendAsync("CustomerSupportMessage", message);
        }
    }
}