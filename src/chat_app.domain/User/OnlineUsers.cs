using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chat_app.domain
{
    public class OnlineUsers
    {
        private readonly List<OnlineUser> _onlineUsers;

        public OnlineUsers()
        {
            _onlineUsers = new List<OnlineUser> (100);
        }

        public bool IsOnline(string name)
        {
            return _onlineUsers.Any (x => x.Name == name);
        }

        public bool IsOnline(Guid id)
        {
            return _onlineUsers.Any (x => x.Id == id);
        }

        public IEnumerable<OnlineUser> List => _onlineUsers;

        public void AddOnlineUser(Guid id, string name)
        {
            if(!_onlineUsers.Any(x => x.Id == id))
            {
                _onlineUsers.Add (new OnlineUser { Id = id, Name = name });
            }
        }

        public void RemoveOnlineUser(Guid id)
        {
            var toRemove = _onlineUsers.SingleOrDefault (x => x.Id == id);

            if(toRemove != null)
            {
                _onlineUsers.Remove (toRemove);
            }
        }
    }
}
