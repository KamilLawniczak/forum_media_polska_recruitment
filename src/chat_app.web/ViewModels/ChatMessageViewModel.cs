using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using chat_app.domain;

namespace chat_app.web.ViewModels
{
    public class ChatMessageViewModel
    {
        public static ChatMessageViewModel ForMessageSender(PrivateMessage message)
        {
            return new ChatMessageViewModel
            {
                ConversationId = message.ReceiverId.ToString (),
                SenderId = message.SenderId,
                Sended = message.Sended,
                Received = message.Received,
                Content = message.Text
            };
        }

        public static ChatMessageViewModel ForMessageReceiver(PrivateMessage message)
        {
            return new ChatMessageViewModel
            {
                ConversationId = message.SenderId.ToString (),
                SenderId = message.SenderId,
                Sended = message.Sended,
                Received = message.Received,
                Content = message.Text
            };
        }

        public static ChatMessageViewModel ForPublic(PublicMessage message)
        {
            return new ChatMessageViewModel
            {
                ConversationId = "Public",
                SenderId = message.SenderId,
                Sended = message.When,
                Received = message.When,
                Content = message.Text
            };
        }

        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public string ConversationId { get; set; }
        public Guid SenderId { get; set; }
        public DateTimeOffset Sended { get; set; }
        public DateTimeOffset Received { get; set; }
        public string Content { get; set; }

        public override string ToString() => JsonSerializer.Serialize (this, _jsonSerializerOptions);
    }
}
