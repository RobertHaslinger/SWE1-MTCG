using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Server
{
    public class RequestContext
    {
        public HttpMethod HttpMethod { get; private set; }

        public string HttpVersion { get; private set; }

        public string RequestedResource { get; private set; }

        public Dictionary<string, string> Headers { get; private set; }

        public int Payload { get; private set; }


        public RequestContext(string header)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            throw new System.NotImplementedException();
        }
    }
}