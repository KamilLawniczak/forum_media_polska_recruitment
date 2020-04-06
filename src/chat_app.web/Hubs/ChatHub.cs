using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
