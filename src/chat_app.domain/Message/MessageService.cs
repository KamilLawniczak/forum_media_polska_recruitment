using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chat_app.domain.Data;
using Microsoft.EntityFrameworkCore;

namespace chat_app.domain
{
    public class MessageService
    {
        private readonly Func<ChatContext> _mkChatContext;

        public MessageService(Func<ChatContext> mkChatContext)
        {
            _mkChatContext = mkChatContext;
        }

        public async Task<IEnumerable<PublicMessage>> GetTopPublicMessages(int count = 20, int page = 1)
        {
            using (var tx = _mkChatContext ())
            {
                return await tx.PublicMessages
                               .AsNoTracking ()
                               .OrderByDescending (x => x.When)
                               .Skip ((page - 1) * count)
                               .Take (count)
                               .ToArrayAsync ();
            }
        }

        public async Task<PublicMessage> AddPublicMessage(Guid senderId, string text)
        {
            var message = new PublicMessage
            {
                Id = Guid.NewGuid (),
                SenderId = senderId,
                When = DateTimeOffset.Now,
                Text = text
            };

            using (var tx = _mkChatContext ())
            {
                tx.PublicMessages.Add (message);
                await tx.SaveChangesAsync ();
            }

            return message;
        }
    }
}
