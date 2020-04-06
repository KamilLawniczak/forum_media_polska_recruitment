using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chat_app.domain;
using chat_app.web.Hubs;
using chat_app.web.Models;
using chat_app.web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace chat_app.web.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly MessageService _messageService;
        private readonly OnlineUsers _onlineUsers;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(MessageService messageService, OnlineUsers onlineUsers, IHubContext<ChatHub> hubContext)
        {
            _messageService = messageService;
            _onlineUsers = onlineUsers;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            var id = Guid.Parse(HttpContext.User.Claims.Single (x => x.Type == "UserId").Value);
            var name = HttpContext.User.Claims.Single (x => x.Type == "UserName").Value;

            if (!_onlineUsers.IsOnline (id))
            {
                _onlineUsers.AddOnlineUser (id, name);
            }

            var users = _onlineUsers.List.ToList ();
            var messages = await _messageService.GetTopPublicMessages ();

            var conversations = new List<Conversation>
            {
                new Conversation
                {
                    Id = "Public",
                    Messages = messages.Select(x => new ChatMessageViewModel
                    {
                        ConversationId = "Public",
                        SenderId = x.SenderId,
                        Sended = x.When,
                        Received = x.When,
                        Content = x.Text
                    }).ToList()
                },

                new Conversation
                {
                    Id = "Test",
                    Messages = messages.Select(x => new ChatMessageViewModel
                    {
                        ConversationId = "Public",
                        SenderId = x.SenderId,
                        Sended = x.When,
                        Received = x.When,
                        Content = x.Text
                    }).ToList()
                },
            };

            conversations[0].Messages.Add (new ChatMessageViewModel { SenderId = Guid.NewGuid(), Content = "hahaha"});
            conversations[1].Messages.Add (new ChatMessageViewModel { SenderId = Guid.NewGuid(), Content = "dsfdfs"});
            var vm = new ChatViewModel
            {
                OnlineUsers = users,
                Conversations =  conversations
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> SendPublicMessage([FromBody] SendPublicMessageModel sendPublicMessageModel)
        {
            var msg = await _messageService.AddPublicMessage (GetUserId(), sendPublicMessageModel.Content);

            await _hubContext.Clients.All.SendAsync ("ReceivePublicMessage", new ChatMessageViewModel
            {
                ConversationId = "Public",
                Content = msg.Text,
                SenderId = msg.SenderId,
                Received = msg.When,
                Sended = msg.When
            });

            return Ok ();
        }

        private Guid GetUserId() => Guid.Parse (HttpContext.User.Claims.Single (x => x.Type == "UserId").Value);
    }
}