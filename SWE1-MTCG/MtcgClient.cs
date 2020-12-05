using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SWE1_MTCG.Server;

namespace SWE1_MTCG
{
    public class MtcgClient
    {
        public User User { get; private set; }
        public string SessionToken { get; private set; }

        [JsonIgnore]
        public TcpClient Socket { get; set; }

        public MtcgClient(User user, string sessionToken)
        {
            User = user;
            SessionToken = sessionToken;
        }
    }
}
