using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Api
{
    public class MessageApi : IRestApi
    {
        public ResponseContext Get(object param)
        {
            RequestContext request = (RequestContext) param;
            KeyValuePair<StatusCode, object> responsePair= 

        }

        public ResponseContext Post(object param)
        {
            throw new NotImplementedException();
        }

        public ResponseContext Put(object param)
        {
            throw new NotImplementedException();
        }

        public ResponseContext Delete(object param)
        {
            throw new NotImplementedException();
        }
    }
}
