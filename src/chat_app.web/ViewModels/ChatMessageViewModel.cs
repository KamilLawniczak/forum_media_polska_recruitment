using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chat_app.web.ViewModels
{
    public class ChatMessageViewModel
    {
        public string ConversationId { get; set; }
        public Guid SenderId { get; set; }
        public DateTimeOffset Sended { get; set; }
        public DateTimeOffset Received { get; set; }
        public string Content { get; set; }
    }
}
