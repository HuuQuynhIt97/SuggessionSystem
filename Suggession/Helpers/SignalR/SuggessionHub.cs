using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession.Helpers.SignalR
{
    public class SuggessionHub : Hub
    {
        public SuggessionHub()
        {
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendData(string message)
        {
            await Clients.All.SendAsync("SentMess", message);
        }

        private static int Count = 0;
        public override Task OnConnectedAsync()
        {

            Count++;
            base.OnConnectedAsync();
            Clients.All.SendAsync("updateCount", Count);
            return Task.CompletedTask;
        }

        //public override Task OnDisconnectedAsync(Exception exception)
        //{
        //    Count--;
        //    base.OnDisconnectedAsync(exception);
        //    Clients.All.SendAsync("updateCount", Count);
        //    return Task.CompletedTask;
        //}
    }
}
