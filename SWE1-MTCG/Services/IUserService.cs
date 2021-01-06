using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Client;

namespace SWE1_MTCG.Services
{
    public interface IUserService
    {
        bool IsRegistered(User user);
        bool Register(User user);
        MtcgClient Login(User user);
        Profile ViewProfile(string username);
        bool EditProfile(ref MtcgClient client, Profile profile);
        bool EditStats(MtcgClient client);

        List<string> GetScoreboard(MtcgClient client);
    }
}