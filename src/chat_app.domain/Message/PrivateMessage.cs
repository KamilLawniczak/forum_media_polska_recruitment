using System;

namespace chat_app.domain
{
    public class PrivateMessage
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public DateTimeOffset Sended { get; set; }
        public DateTimeOffset Received { get; set; }
        public string Text { get; set; }
    }
}
