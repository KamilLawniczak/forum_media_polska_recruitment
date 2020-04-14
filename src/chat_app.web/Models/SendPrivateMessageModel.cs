using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chat_app.web
{
    public class SendPrivateMessageModel
    {
        public Guid ReceiverId { get; set; }
        public DateTimeOffset Sended { get; set; }
        public string Content { get; set; }
    }
}
