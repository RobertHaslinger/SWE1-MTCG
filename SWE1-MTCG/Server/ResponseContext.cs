using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Server
{
    public class ResponseContext
    {
        public string RequestUrl { get; private set; }
        public HttpMethod RequestMethod { get; private set; }
        public StatusCode StatusCode { get; private set; }
        public Dictionary<string, string> Headers { get; private set; } = new Dictionary<string, string>();

        public ResponseContext(string requestUrl, HttpMethod requestMethod, StatusCode statusCode)
        {
            RequestUrl = requestUrl;
            RequestMethod = requestMethod;
            StatusCode = statusCode;
        }

        public void AddHeader(string headerKey, string headerValue)
        {
            if (string.IsNullOrWhiteSpace(headerKey)) return;

            Headers.Add(headerKey, headerValue);
        }

        /// <summary>
        /// Builds a fully escaped response header.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
