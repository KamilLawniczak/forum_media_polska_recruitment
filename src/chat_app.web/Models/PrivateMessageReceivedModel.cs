using System;

namespace chat_app.web.Models
{
    public class PrivateMessageReceivedModel
    {
        public Guid MessageId { get; set; }
        public DateTimeOffset Received { get; set; }
    }
}
