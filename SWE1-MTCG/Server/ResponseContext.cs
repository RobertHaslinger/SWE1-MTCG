using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using SWE1_MTCG.Cards;
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
            
            AddHeader("Response", $"{request.HttpVersion} {(int)StatusCode} {Enum.GetName(typeof(StatusCode), StatusCode)}");
            AddHeader("Server", "Robert Haslingers MTCG Web Server");
            AddHeader("Date", DateTime.Now.ToString());
            AddHeader("Cache-Control", "no-cache");
            //TODO change to accept header
            if (!request.QueryParams.ContainsKey("format"))
            {
                SetUpJsonContent(keyValuePair.Value);
            }
            else
            {
                switch (request.QueryParams["format"])
                {
                    case "plain":
                    {
                        SetUpPlainContent(keyValuePair.Value);
                        break;
                    }
                    default:
                    {
                        SetUpJsonContent(keyValuePair.Value);
                        break;
                    } 
                };
            }
            
            AddHeader("Content-Length", Content.Length.ToString());
        }

        private void SetUpPlainContent(object content)
        {
            Type type = content.GetType();
            if (type.IsAssignableTo(typeof(IEnumerable)))
            {
                var listType = type.GetGenericArguments()[0];
                if (listType.FullName == typeof(Card).FullName)
                {
                    Content = string.Join("\r\n",
                        ((List<Card>) content).Select(c =>
                            $"This is {c.Name} ({c.Guid}) with the Element {Enum.GetName(typeof(ElementType), c.Element)} and a damage of {c.Damage}")
                        .ToArray());
                }
            }
            AddHeader("Content-Type", "text/plain");
        }

        private void SetUpJsonContent(object content)
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            Content = JsonSerializer.Serialize(content, options);
            AddHeader("Content-Type", "application/json");
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
            string escapedHeaders = string.Join("\r\n",
                Headers.Select(x => $"{x.Key}: {x.Value}").ToArray());
            return Headers["Response"]+"\r\n"+escapedHeaders+
                "\r\n\r\n"+Content;
        }
    }
}
