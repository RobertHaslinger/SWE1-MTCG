using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Server
{
    public class RequestContext
    {
        private IApiService _apiService;

        public HttpMethod HttpMethod { get; private set; }
        public string HttpVersion { get; private set; }
        public string RequestedApi { get; private set; }
        public string RequestedResource { get; private set; }
        public Dictionary<string, string> Headers { get; private set; }
        public string Payload { get; private set; }

        private const string MethodPattern = "^\\w{3,}";
        private const string VersionPattern = "HTTP/\\d(\\.?\\d+){0,1}";
        /// <summary>
        /// Base: https://www.regextester.com/1965. Modified to match for example /messages/all
        /// </summary>
        private const string FullResourcePattern = "(http[s]?:\\/\\/)?[^\\s([\" <,>]*[\\.\\/][^\\s[\",><]*";

        private const string RequestedApiPattern = "(/\\w+){1}";
        private const string ValuesPattern = "((\\r\\n).+:.+)+(\\r\\n\\r\\n)";
        private const string PayloadPattern = "(\\r\\n\\r\\n)[^\\0]*";


        public RequestContext(string header, IApiService apiService)
        {
            _apiService = apiService;
            Regex methodRegex = new Regex(MethodPattern);
            Regex headerRegex = new Regex(VersionPattern);
            Regex fullResourceRegex = new Regex(FullResourcePattern);
            Regex valuesRegex = new Regex(ValuesPattern);
            Regex payloadRegex = new Regex(PayloadPattern);
            Regex requestedApiRegex= new Regex(RequestedApiPattern);

            HttpMethod = _apiService.GetHttpMethod(methodRegex.Match(header).Value);
            HttpVersion = headerRegex.Match(header).Value;
            string fullResource = fullResourceRegex.Match(header).Value;
            RequestedApi = requestedApiRegex.Match(fullResource).Value;
            RequestedResource = requestedApiRegex.Replace(fullResource, "", 1).TrimStart('/');
            Headers = new Dictionary<string, string>();

            string headerValuePairs = valuesRegex.Match(header).Value;

            foreach (string headerValue in headerValuePairs.Split("\r\n", StringSplitOptions.RemoveEmptyEntries))
            {
                if (string.IsNullOrWhiteSpace(headerValue)) continue;

                string[] valuePair = headerValue.Split(':');
                Headers.Add(valuePair[0], valuePair[1]);
            }

            Payload = payloadRegex.Match(header).Value.TrimStart();
        }

        public override string ToString()
        {
            throw new System.NotImplementedException();
        }
    }
}