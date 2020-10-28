using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Server
{
    public class ResponseContext
    {
        public string RequestUrl { get; private set; }
        public HttpMethod RequestMethod { get; private set; }
        public StatusCode StatusCode { get; set; }
        public Dictionary<string, string> Headers { get; private set; } = new Dictionary<string, string>();
        public string Content { get; set; }

        public ResponseContext(RequestContext request, KeyValuePair<StatusCode, object> keyValuePair)
        {
            RequestUrl = $"{request.RequestedApi}/{request.RequestedResource}";
            RequestMethod = request.HttpMethod;
            StatusCode = keyValuePair.Key;
            Content = keyValuePair.Value.ToString();
            AddHeader("Response", $"{request.HttpVersion} {(int)StatusCode} {Enum.GetName(typeof(StatusCode), StatusCode)}");
            AddHeader("Server", "Robert Haslingers Web Server");
            AddHeader("Date", DateTime.Now.ToString());
            AddHeader("Cache-Control", "no-cache");
            AddHeader("Content-Type", "text/plain");
            AddHeader("Content-Length", Content.Length.ToString());
        }

        public void AddHeader(string headerKey, string headerValue)
        {
            if (string.IsNullOrWhiteSpace(headerKey)) return;

            Headers.Add(headerKey, headerValue);
        }

        /// <summary>
        /// Builds a fully escaped response.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Headers["Response"]+"\r\n"+string.Join("\r\n", Headers.Select(x => string.Format("{0}: {1}", x.Key, x.Value).ToArray())+
                "\r\n"+Content);
        }
    }
}
