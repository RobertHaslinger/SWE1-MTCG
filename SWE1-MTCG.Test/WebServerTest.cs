using System;
using System.Collections.Generic;
using System.Net;
using Moq;
using NUnit.Framework;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;
using SWE1_MTCG.Test.TestCaseData;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class WebServerTest
    {
        private WebServer _server;
        private IApiService _apiService;

        [SetUp]
        public void SetUp()
        {
            _apiService = new ApiService();
            _server= new WebServer(IPAddress.Loopback, 10001, _apiService);
        }

        [Test, TestCaseSource(typeof(WebServerTestCaseData), nameof(WebServerTestCaseData.TestCases))]
        public Type Test_ApiServiceReturnsExpectedRestApiFromRequest(string requestedApi)
        {
            return _apiService.GetRequestedApi(requestedApi).GetType();
        }

        [Test, TestCaseSource(typeof(WebServerTestCaseData), nameof(WebServerTestCaseData.ExceptionalTestCases))]
        public void Test_ApiServiceShouldThrowExceptionIfNoValidRequestedApi(string requestedApi)
        {
            Assert.Throws(typeof(KeyNotFoundException), () => _apiService.GetRequestedApi(requestedApi));
        }
    }
}