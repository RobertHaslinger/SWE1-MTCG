using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Client
{
    public class MtcgClient
    {
        public User User { get; private set; }
        public string SessionToken { get; private set; }

        public RequestContext CurrentRequest { get; set; }

        public MtcgClient(User user, string sessionToken)
        {
            User = user;
            SessionToken = sessionToken;
        }
    }
}
