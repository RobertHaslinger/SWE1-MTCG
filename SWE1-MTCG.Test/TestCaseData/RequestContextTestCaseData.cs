using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Test.TestCaseData
{
    public class RequestContextTestCaseData
    {
        public static IEnumerable TestCases
        {
            get
            {
                //GET /messages
                yield return new NUnit.Framework.TestCaseData(
                    "GET /messages HTTP/1.1\r\nUser-Agent: PostmanRuntime/7.26.8\r\n" +
                    "Accept: */*\r\nCache-Control: no-cache\r\nPostman-Token: 0ee11733-7510-4087-a790-75016d5bd625\r\n" +
                    "Host: localhost:10001\r\nAccept-Encoding: gzip, deflate, br\r\n" +
                    "Connection: keep-alive\r\n\r\n", 
                    new RequestContextTestResult(Enum.GetName(typeof(HttpMethod), HttpMethod.Get), "/messages", string.Empty, "HTTP/1.1", 7, string.Empty));

                //GET /messages/1
                yield return new NUnit.Framework.TestCaseData(
                    "GET /messages/1 HTTP/1.1\r\nUser-Agent: PostmanRuntime/7.26.8\r\n" +
                    "Accept: */*\r\nCache-Control: no-cache\r\nPostman-Token: de939d81-e62d-4014-aca1-34aacac6a385\r\n" +
                    "Host: localhost:10001\r\nAccept-Encoding: gzip, deflate, br\r\nConnection: keep-alive\r\n\r\n",
                    new RequestContextTestResult(Enum.GetName(typeof(HttpMethod), HttpMethod.Get), "/messages", "1", "HTTP/1.1", 7, string.Empty));

                //POST /messages
                yield return new NUnit.Framework.TestCaseData(
                    "POST /messages HTTP/1.1\r\nContent-Type: text/plain\r\n" +
                    "User-Agent: PostmanRuntime/7.26.8\r\nAccept: */*\r\nCache-Control: no-cache\r\n" +
                    "Postman-Token: cb73f51e-5cbe-421c-a4ad-093e01b0a61e" +
                    "\r\nHost: localhost:10001\r\nAccept-Encoding: gzip, deflate, br\r\nConnection: keep-alive\r\n" +
                    "Content-Length: 21\r\n\r\nsuper tolle nachricht",
                    new RequestContextTestResult(Enum.GetName(typeof(HttpMethod), HttpMethod.Post), "/messages", string.Empty, "HTTP/1.1", 9, "super tolle nachricht"));

                //PUT /messages/7
                yield return new NUnit.Framework.TestCaseData(
                    "PUT /messages/7 HTTP/1.1\r\nContent-Type: text/plain\r\n" +
                    "User-Agent: PostmanRuntime/7.26.8\r\nAccept: */*\r\nCache-Control: no-cache\r\n" +
                    "Postman-Token: c4bf57e4-d568-4e4d-a278-64d4f061c266\r\nHost: localhost:10001\r\n" +
                    "Accept-Encoding: gzip, deflate, br\r\nConnection: keep-alive\r\nContent-Length: 12\r\n\r\n7. Nachricht",
                    new RequestContextTestResult(Enum.GetName(typeof(HttpMethod), HttpMethod.Put), "/messages", "7", "HTTP/1.1", 9, "7. Nachricht"));

                //DELETE /messages/10
                yield return new NUnit.Framework.TestCaseData(
                    "DELETE /messages/10 HTTP/1.1\r\nUser-Agent: PostmanRuntime/7.26.8\r\n" +
                    "Accept: */*\r\nCache-Control: no-cache\r\nPostman-Token: c9eede90-dbac-4378-b584-309018bd9e73\r\n" +
                    "Host: localhost:10001\r\nAccept-Encoding: gzip, deflate, br\r\nConnection: keep-alive\r\n\r\n",
                    new RequestContextTestResult(Enum.GetName(typeof(HttpMethod), HttpMethod.Delete), "/messages", "10", "HTTP/1.1", 7, string.Empty));
            }
        }
    }
}
