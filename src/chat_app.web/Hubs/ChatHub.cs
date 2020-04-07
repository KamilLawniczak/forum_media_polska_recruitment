using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chat_app.domain;
using chat_app.web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace chat_app.web.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly OnlineUsers _onlineUsers;
        public ChatHub(OnlineUsers onlineUsers)
        {
            _onlineUsers = onlineUsers;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Guid.Parse (Context.User.Claims.Single (x => x.Type == "UserId").Value);
            var connectionId = Context.ConnectionId;

            _onlineUsers.MapChatHubConnection (userId, connectionId);

            if(_onlineUsers.CountChatHubConnections (userId) == 1)
            {
                await Clients.All.SendAsync ("ChatUserConnected", _onlineUsers.GetUser(userId));
            }

            await base.OnConnectedAsync ();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Guid.Parse (Context.User.Claims.Single (x => x.Type == "UserId").Value);
            var connectionId = Context.ConnectionId;

            _onlineUsers.UnmapChatHubConnection (userId, connectionId);

            if (_onlineUsers.CountChatHubConnections (userId) < 1)
            {
                await Clients.All.SendAsync ("ChatUserDisconnected", userId);
            }

            await base.OnDisconnectedAsync (exception);
        }

        public async Task SendPublicMessage(ChatMessageViewModel message)
        {
            await Clients.All.SendAsync ("ReceivePublicMessage", message);
        }
    }
}
