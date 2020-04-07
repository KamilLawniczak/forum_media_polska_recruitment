using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace chat_app.domain
{
    public class OnlineUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        [JsonIgnore]
        public HashSet<string> ChatHubConnections { get; } = new HashSet<string> ();
    }
}
