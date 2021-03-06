﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Server
{
    /// <summary>
    /// I found the Regex patterns here: https://www.google.com/search?q=cat+keyboard+gif&sxsrf=ALeKk01pgwWNx0L8RrT3n4mzgSudgc9K6Q:1603974510220&tbm=isch&source=iu&ictx=1&fir=zXl3YAU62uJkwM%252Cx4ryeLiJmrIWyM%252C_&vet=1&usg=AI4_-kSgKsMnGEJcshbXtQ6uSg08e4jZBQ&sa=X&ved=2ahUKEwii0aPX5tnsAhUB66QKHR5dAaAQ9QF6BAgDECE&biw=1920&bih=937#imgrc=zXl3YAU62uJkwM
    /// </summary>
    public class RequestContext
    {
        private IApiService _apiService;

        public HttpMethod HttpMethod { get; private set; }
        public string HttpVersion { get; private set; }
        public string RequestedApi { get; private set; }
        public string RequestedResource { get; private set; }
        public Dictionary<string, string> QueryParams { get; private set; }
        public Dictionary<string, string> Headers { get; private set; }
        public string Payload { get; private set; }

        private const string MethodPattern = "^\\w{3,}";
        private const string VersionPattern = "HTTP/\\d(\\.?\\d+){0,1}";
        /// <summary>
        /// Base: https://www.regextester.com/1965. Modified to match for example /messages/all
        /// </summary>
        private const string FullResourcePattern = "(http[s]?:\\/\\/)?[^\\s([\" <,>]*[\\.\\/][^\\s[\",><]*";

        private const string RequestedApiPattern = "((\\/\\w+){1,}\\/*[^\\d](?=(([0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12})|[0-9]*$|\\?))){1,}";
        private const string QueryParamsPattern = "(\\?\\w+\\=\\w+){1}(\\&\\w+\\=\\w+)*";
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
            Regex queryParamsRegex= new Regex(QueryParamsPattern);

            try
            {
                HttpMethod = _apiService.GetHttpMethod(methodRegex.Match(header).Value);
            }
            catch (KeyNotFoundException e)
            {
                HttpMethod = HttpMethod.Unrecognized;
            }
            HttpVersion = headerRegex.Match(header).Value;
            string fullResource = fullResourceRegex.Match(header).Value;
            RequestedApi = requestedApiRegex.Match(fullResource).Value.TrimEnd('/', '?');

            QueryParams= new Dictionary<string, string>();
            string queryParamPairs = queryParamsRegex.Match(fullResource).Value.TrimStart('?');
            foreach (string queryParam in queryParamPairs.Split('&', StringSplitOptions.RemoveEmptyEntries))
            {
                if (string.IsNullOrWhiteSpace(queryParam)) continue;

                string[] valuePair = queryParam.Split('=');
                QueryParams.Add(valuePair[0].Trim(), valuePair[1].Trim());
            }
            RequestedResource = requestedApiRegex.Replace(queryParamsRegex.Replace(fullResource, "", 1), "", 1).TrimStart('/');
            Headers = new Dictionary<string, string>();

            string headerValuePairs = valuesRegex.Match(header).Value;

            foreach (string headerValue in headerValuePairs.Split("\r\n", StringSplitOptions.RemoveEmptyEntries))
            {
                if (string.IsNullOrWhiteSpace(headerValue)) continue;

                string[] valuePair = headerValue.Split(':');
                Headers.Add(valuePair[0].Trim(), valuePair[1].Trim());
            }

            Payload = payloadRegex.Match(header).Value.TrimStart();
            Console.WriteLine(this);
        }

        public override string ToString()
        {
            return
                $"{Enum.GetName(typeof(HttpMethod), HttpMethod)} {RequestedApi}/{RequestedResource} {HttpVersion}\r\n" +
                string.Join("\r\n", Headers.Select(h => $"{h.Key}:{h.Value}").ToArray()) +
                $"\r\n\r\n{Payload}";
        }
    }
}