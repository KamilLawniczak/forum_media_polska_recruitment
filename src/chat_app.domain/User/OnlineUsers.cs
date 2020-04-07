using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static chat_app.domain.Utils.ComparerHelper;

namespace chat_app.domain
{
    public class OnlineUsers
    {
        private readonly HashSet<OnlineUser> _onlineUsers;

        public OnlineUsers()
        {
            var equalityComparer = CreateEqualityComparer<OnlineUser> (
                (x, y) => x.Id == y.Id,
                x => x.Id.GetHashCode () ^ 197 * x.Name.GetHashCode ());

            _onlineUsers = new HashSet<OnlineUser> (equalityComparer);
        }

        public bool IsOnline(string name) => _onlineUsers.Any (x => x.Name == name);

        public bool IsOnline(Guid id) => _onlineUsers.Any (x => x.Id == id);

        public IEnumerable<OnlineUser> List => _onlineUsers;

        public OnlineUser GetUser(Guid id) => _onlineUsers.Single (x => x.Id == id);

        public void AddOnlineUser(Guid id, string name) => _onlineUsers.Add (new OnlineUser { Id = id, Name = name });

        public void RemoveOnlineUser(Guid id)
        {
            var toRemove = _onlineUsers.SingleOrDefault (x => x.Id == id);

            if (toRemove != null)
            {
                _onlineUsers.Remove (toRemove);
            }
        }

        public void MapChatHubConnection(Guid userId, string connecionId)
        {
            var user = _onlineUsers.Single (x => x.Id == userId);
            user.ChatHubConnections.Add (connecionId);
        }

        public void UnmapChatHubConnection(Guid userId, string connectionId)
        {
            var user = _onlineUsers.Single (x => x.Id == userId);
            user.ChatHubConnections.Remove (connectionId);
        }

        public IEnumerable<string> GetChatHubConnections(Guid userId) => _onlineUsers.Single (x => x.Id == userId).ChatHubConnections;
        public int CountChatHubConnections(Guid userId) => _onlineUsers.Single (x => x.Id == userId).ChatHubConnections.Count;
    }
}
