using System.Collections.Generic;

namespace chat_app.web.ViewModels
{
    public class Conversation
    {
        public string Id { get; set; }
        public List<ChatMessageViewModel> Messages { get; set; }
    }
}