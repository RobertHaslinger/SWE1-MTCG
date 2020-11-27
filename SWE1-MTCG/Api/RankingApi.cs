using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Api
{
    public class RankingApi : IRestApi
    {
        public bool AllowAnonymous => true;

        public ResponseContext Get(Dictionary<string, object> param)
        {
            throw new NotImplementedException();
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
