using System;

namespace chat_app.domain
{
    public class PublicMessage
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public DateTimeOffset When { get; set; }
        public string Text { get; set; }
    }
}
