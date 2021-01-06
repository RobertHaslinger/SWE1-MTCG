using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Client;
using SWE1_MTCG.Controller;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Api
{
    public class RankingApi : IRestApi
    {
        private UserController _userController;
        public bool AllowAnonymous => false;

        public RankingApi()
        {
            IUserService userService = new UserService();
            _userController = new UserController(userService);
        }

        public ResponseContext Get(Dictionary<string, object> param)
        {
            RequestContext request = (RequestContext)param["request"];
            MtcgClient client = (MtcgClient)param["client"];

            return new ResponseContext(request, _userController.ViewScoreboard(client));
        }

        public ResponseContext Post(Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public ResponseContext Put(Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public ResponseContext Delete(Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }
    }
}
