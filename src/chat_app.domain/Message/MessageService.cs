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

        public async Task<IEnumerable<PublicMessage>> GetPublicMessages(int count = 20, int skip = 0)
        {
            using (var tx = _mkChatContext ())
            {
                return await tx.PublicMessages
                               .AsNoTracking ()
                               .OrderByDescending (x => x.When)
                               .Skip (skip)
                               .Take (count)
                               .ToArrayAsync ();
            }
        }

        public async Task<IEnumerable<PrivateMessage>> GetPrivateMessages(
            Guid user1Id,
            Guid user2Id,
            int count = 20,
            int skip = 0
            )
        {
            using (var tx = _mkChatContext ())
            {
                return await tx.PrivateMessages
                               .AsNoTracking ()
                               .Where (x => (x.SenderId == user1Id && x.ReceiverId == user2Id)
                                         || (x.SenderId == user2Id && x.ReceiverId == user1Id))
                               .OrderByDescending (x => x.Sended)
                               .ThenByDescending (x => x.Received)
                               .Skip (skip)
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

        public async Task<PrivateMessage> AddPrivateMessage(Guid senderId, Guid receiverId, DateTimeOffset sended, string text)
        {
            var msg = new PrivateMessage
            {
                Id = Guid.NewGuid(),
                SenderId = senderId,
                ReceiverId = receiverId,
                Sended = sended,
                Received = DateTimeOffset.MinValue,
                Text = text
            };

            using (var tx = _mkChatContext ())
            {
                tx.PrivateMessages.Add (msg);
                await tx.SaveChangesAsync ();
            }

            return msg;
        }

        public async Task<PrivateMessage> UpdateReceivedTime(Guid id, DateTimeOffset received)
        {
            using (var tx = _mkChatContext())
            {
                var user = await tx.PrivateMessages.FindAsync (id);
                
                if(user != null)
                {
                    user.Received = received;
                    await tx.SaveChangesAsync ();
                }

                return user;
            }
        }
    }
}
