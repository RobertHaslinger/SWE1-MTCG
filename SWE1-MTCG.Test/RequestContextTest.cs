using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;
using SWE1_MTCG.Test.TestCaseData;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class RequestContextTest
    {
        private ApiService _apiService;

        [OneTimeSetUp]
        public void Setup()
        {
            _apiService= new ApiService();
        }

        [Test, TestCaseSource(typeof(RequestContextTestCaseData), nameof(RequestContextTestCaseData.TestCases))]
        public void Test_RequestContextPropertiesAreEqualToExceptedResult(string requestString,
            RequestContextTestResult expectedResult)
        {
            RequestContext requestContext= new RequestContext(requestString, _apiService);

            Assert.AreEqual(expectedResult.HttpMethod, Enum.GetName(typeof(HttpMethod), requestContext.HttpMethod) );
            Assert.AreEqual(expectedResult.RequestedApi, requestContext.RequestedApi);
            Assert.AreEqual(expectedResult.RequestedResource, requestContext.RequestedResource);
            Assert.AreEqual(expectedResult.HttpVersion, requestContext.HttpVersion);
            Assert.AreEqual(expectedResult.HeaderCount, requestContext.Headers.Count);
            Assert.AreEqual(expectedResult.Payload, requestContext.Payload);
        }
    }

    public record RequestContextTestResult 
    {
        public string HttpMethod { get; }
        public string RequestedApi { get; }
        public string RequestedResource { get; }
        public string HttpVersion { get; }
        public int HeaderCount { get; }
        public string Payload { get; }

    public RequestContextTestResult(string httpMethod, string requestedApi, string requestedResource,
        string httpVersion, int headerCount, string payload)
        => (HttpMethod, RequestedApi, RequestedResource, HttpVersion, HeaderCount, Payload) =
            (httpMethod, requestedApi, requestedResource, httpVersion, headerCount, payload);
    }
}