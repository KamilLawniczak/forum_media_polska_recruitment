using System;
using System.Threading.Tasks;
using chat_app.domain.Data;
using Microsoft.EntityFrameworkCore;

namespace chat_app.domain
{
    public class ChatUserService
    {
        private readonly Func<ChatContext> _mkCtx;
        private readonly ISecurePasswordService _securePasswordService;

        public ChatUserService(Func<ChatContext> mkCtx, ISecurePasswordService securePasswordService)
        {
            _mkCtx = mkCtx;
            _securePasswordService = securePasswordService;
        }

        public async Task AddNewUser(string userName, string password)
        {
            using (var tx = _mkCtx ())
            {
                var alreadyExists = await tx.ChatUsers.AnyAsync (x => x.Name == userName);
                if (alreadyExists) throw new ArgumentException ("Provided user already exists");

                var hashAndSalt = _securePasswordService.ComputeForUser (userName, password);
                var user = new ChatUser
                {
                    Id = Guid.NewGuid (),
                    Name = userName,
                    PasswordHash = hashAndSalt.passwordHash,
                    PasswordSalt = hashAndSalt.salt
                };

                tx.ChatUsers.Add (user);
                await tx.SaveChangesAsync ();
            }
        }

        public async Task<(bool success, ChatUser user)> TryLogIn(string userName, string password)
        {
            using (var tx = _mkCtx ())
            {
                var user = await tx.ChatUsers.SingleOrDefaultAsync (x => x.Name == userName);

                if (user == null) return (false, null);

                var success = _securePasswordService.ChallengePassword (userName, password, user.PasswordHash, user.PasswordSalt);

                return (success, success ? user : null);
            }
        }
    }
}
