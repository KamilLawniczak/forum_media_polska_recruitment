using System;

namespace chat_app.web.Models
{
    public class PrivateMessagesPageQueryModel
    {
        public Guid InterlocutorId { get; set; }

        public int Skip { get; set; }

        public int Count { get; set; }
    }
}
