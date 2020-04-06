using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using chat_app.domain;

namespace chat_app.web.ViewModels
{
    public class ChatViewModel
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public List<OnlineUser> OnlineUsers { get; set; }

        public List<Conversation> Conversations { get; set; }

        public override string ToString() => JsonSerializer.Serialize (this, _jsonSerializerOptions);
    }
}
