using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        private readonly MessageService _messageService;
        private readonly OnlineUsers _onlineUsers;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(MessageService messageService, OnlineUsers onlineUsers, IHubContext<ChatHub> hubContext)
        {
            _messageService = messageService;
            _onlineUsers = onlineUsers;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var id = Guid.Parse (HttpContext.User.Claims.Single (x => x.Type == "UserId").Value);
            var name = HttpContext.User.Claims.Single (x => x.Type == "UserName").Value;

            if (!_onlineUsers.IsOnline (id))
            {
                _onlineUsers.AddOnlineUser (id, name);
            }

            var users = _onlineUsers.List.ToList ();
            var messages = await _messageService.GetPublicMessages ();

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
            };

            var vm = new ChatViewModel
            {
                OnlineUsers = users,
                Conversations = conversations
            };

            return View (vm);
        }

        [HttpGet]
        public async Task<IActionResult> GetPrivateMessagesPage([FromQuery]PrivateMessagesPageQueryModel privateMessagesPageQueryModel)
        {
            var currentUserId = GetUserId ();
            var messages = await _messageService.GetPrivateMessages (
                currentUserId,
                privateMessagesPageQueryModel.InterlocutorId,
                privateMessagesPageQueryModel.Count,
                privateMessagesPageQueryModel.Skip);

            var result = messages.Select (m => new ChatMessageViewModel
            {
                ConversationId = privateMessagesPageQueryModel.InterlocutorId.ToString (),
                SenderId = m.SenderId,
                Sended = m.Sended,
                Received = m.Received,
                Content = m.Text
            });

            return Json (result, _jsonSerializerOptions);
        }

        [HttpPost]
        public async Task<IActionResult> SendPublicMessage([FromBody] SendPublicMessageModel sendPublicMessageModel)
        {
            var msg = await _messageService.AddPublicMessage (GetUserId (), sendPublicMessageModel.Content);

            await _hubContext.Clients.All.SendAsync ("ReceivePublicMessage", ChatMessageViewModel.ForPublic(msg));

            return Ok ();
        }

        [HttpPost]
        public async Task<IActionResult> SendPrivateMessage([FromBody] SendPrivateMessageModel sendPrivateMessageModel)
        {
            var currentUserId = GetUserId ();

            var privateMessage = await _messageService.AddPrivateMessage (
                                        currentUserId,
                                        sendPrivateMessageModel.ReceiverId,
                                        sendPrivateMessageModel.Sended,
                                        sendPrivateMessageModel.Content);

            var todoSender = CreateMessageSendingTasks (
                _onlineUsers.GetChatHubConnections (currentUserId),
                ChatMessageViewModel.ForMessageSender (privateMessage),
                _hubContext);

            var todoReceiver = CreateMessageSendingTasks (
                _onlineUsers.GetChatHubConnections (privateMessage.ReceiverId),
                ChatMessageViewModel.ForMessageReceiver (privateMessage),
                _hubContext);

            await Task.WhenAll (todoSender.Concat (todoReceiver));

            return Ok ();
        }

        [HttpPost]
        public async Task<IActionResult> PrivateMessageReceived([FromBody] PrivateMessageReceivedModel privateMessageReceivedModel)
        {
            await _messageService.UpdateReceivedTime (privateMessageReceivedModel.MessageId, privateMessageReceivedModel.Received);

            return Ok ();
        }

        private Guid GetUserId() => Guid.Parse (HttpContext.User.Claims.Single (x => x.Type == "UserId").Value);

        private static IEnumerable<Task> CreateMessageSendingTasks(
            IEnumerable<string> hubConnections,
            ChatMessageViewModel messageViewModel,
            IHubContext<ChatHub> hubContext)
        {
            return hubConnections.Select (
                x => Task.Run (async () => {
                    await hubContext.Clients.Client (x).SendAsync ("ReceivePrivateMessage", messageViewModel);
                    return true;
                }));
        }
    }
}