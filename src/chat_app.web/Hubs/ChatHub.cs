using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chat_app.web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace chat_app.web.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private static readonly Dictionary<string, string> _Connections;

        public override Task OnConnectedAsync()
        {
            
            return base.OnConnectedAsync ();
        }

        public async Task SendPublicMessage(ChatMessageViewModel message)
        {
            await Clients.All.SendAsync ("ReceivePublicMessage", message);
        }
    }
}
